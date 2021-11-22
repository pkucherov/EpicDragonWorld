using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 7th 2019
 */
public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    public static readonly int VISIBILITY_RADIUS = 10000; // This is the maximum allowed visibility radius.
    public static readonly object UPDATE_OBJECT_LOCK = new object();
    private static readonly object UPDATE_METHOD_LOCK = new object();

    private DynamicCharacterAvatar _activeCharacter;
    private WorldObject _targetWorldObject;
    private bool _isPlayerInWater = false;
    private bool _isPlayerOnTheGround = false;
    private bool _kickFromWorld = false;
    private bool _exitingWorld = false;
    private ConcurrentDictionary<long, GameObject> _gameObjects;
    private ConcurrentDictionary<long, MovementHolder> _moveQueue;
    private ConcurrentDictionary<long, AnimationHolder> _animationQueue;
    private ConcurrentDictionary<long, CharacterDataHolder> _characterUpdateQueue;
    private List<long> _deleteQueue;

    private void Start()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        _gameObjects = new ConcurrentDictionary<long, GameObject>();
        _moveQueue = new ConcurrentDictionary<long, MovementHolder>();
        _animationQueue = new ConcurrentDictionary<long, AnimationHolder>();
        _characterUpdateQueue = new ConcurrentDictionary<long, CharacterDataHolder>();
        _deleteQueue = new List<long>();

        if (MainManager.Instance.GetSelectedCharacterData() != null)
        {
            // Create player character.
            _activeCharacter = CharacterManager.Instance.CreateCharacter(MainManager.Instance.GetSelectedCharacterData());
            // Tempfix.
            // TODO: Find why character initializes at 0, 0.
            // TODO: Try initialize with delay?
            Destroy(_activeCharacter.gameObject);
            _activeCharacter = CharacterManager.Instance.CreateCharacter(MainManager.Instance.GetSelectedCharacterData());

            // Animations.
            _activeCharacter.gameObject.AddComponent<AnimationController>();

            // Movement.
            _activeCharacter.gameObject.AddComponent<MovementController>();

            // Add name text.
            WorldObjectText worldObjectText = _activeCharacter.gameObject.AddComponent<WorldObjectText>();
            worldObjectText.SetWorldObjectName(""); // MainManager.Instance.selectedCharacterData.GetName();
            worldObjectText.SetAttachedObject(_activeCharacter.gameObject);

            // Update status information.
            StatusInformationManager.Instance.UpdatePlayerInformation();

            // Send enter world to Network.
            NetworkManager.SendPacket(new EnterWorldRequest(MainManager.Instance.GetSelectedCharacterData().GetName()));
        }
    }

    private void Update()
    {
        if (_exitingWorld)
        {
            return;
        }
        if (_kickFromWorld)
        {
            ExitWorld();
            MainManager.Instance.LoadScene(MainManager.LOGIN_SCENE);
            return;
        }

        lock (UPDATE_METHOD_LOCK)
        {
            // Distance check.
            foreach (GameObject obj in _gameObjects.Values)
            {
                if (obj != null && CalculateDistance(obj.transform.position) > VISIBILITY_RADIUS)
                {
                    WorldObject worldObject = obj.GetComponent<WorldObject>();
                    if (worldObject != null && !_deleteQueue.Contains(worldObject.GetObjectId()))
                    {
                        _deleteQueue.Add(worldObject.GetObjectId());
                    }
                }
            }

            // Delete pending objects.
            foreach (long objectId in _deleteQueue)
            {
                if (_gameObjects.ContainsKey(objectId))
                {
                    GameObject obj = _gameObjects[objectId];
                    if (obj != null)
                    {
                        // Disable.
                        obj.GetComponent<WorldObjectText>().GetNameMesh().gameObject.SetActive(false);
                        obj.SetActive(false);

                        // Remove from objects list.
                        ((IDictionary<long, GameObject>)_gameObjects).Remove(obj.GetComponent<WorldObject>().GetObjectId());

                        // Delete game object from world with a delay.
                        StartCoroutine(DelayedDestroy(obj));
                    }
                }

                // If object was current target, unselect it.
                if (_targetWorldObject != null && _targetWorldObject.GetObjectId() == objectId)
                {
                    SetTarget(null);
                }
            }
            if (_deleteQueue.Count > 0)
            {
                _deleteQueue.Clear();
            }

            // Move pending objects.
            foreach (KeyValuePair<long, MovementHolder> entry in _moveQueue)
            {
                Vector3 position = new Vector3(entry.Value.GetX(), entry.Value.GetY(), entry.Value.GetZ());
                if (_gameObjects.ContainsKey(entry.Key))
                {
                    GameObject obj = _gameObjects[entry.Key];
                    if (obj != null)
                    {
                        WorldObject worldObject = obj.GetComponent<WorldObject>();
                        if (worldObject != null)
                        {
                            if (CalculateDistance(position) > VISIBILITY_RADIUS) // Moved out of sight.
                            {
                                // Broadcast self position, object out of sight.
                                NetworkManager.SendPacket(new LocationUpdateRequest(MovementController.GetStoredPosition().x, MovementController.GetStoredPosition().y, MovementController.GetStoredPosition().z, MovementController.GetStoredRotation()));
                                _deleteQueue.Add(worldObject.GetObjectId());
                            }
                            else
                            {
                                worldObject.MoveObject(position, entry.Value.GetHeading());
                            }
                        }
                    }
                }
                // Check self teleporting.
                else if (entry.Key == 0)
                {
                    _activeCharacter.transform.localPosition = position;
                    _activeCharacter.GetComponent<Rigidbody>().position = position;
                }
                // Request unknown object info from server.
                else if (CalculateDistance(position) <= VISIBILITY_RADIUS)
                {
                    NetworkManager.SendPacket(new ObjectInfoRequest(entry.Key));
                    // Broadcast self position, in case player is not moving.
                    NetworkManager.SendPacket(new LocationUpdateRequest(MovementController.GetStoredPosition().x, MovementController.GetStoredPosition().y, MovementController.GetStoredPosition().z, MovementController.GetStoredRotation()));
                }

                ((IDictionary<long, MovementHolder>)_moveQueue).Remove(entry.Key);
            }

            // Animate pending objects.
            foreach (KeyValuePair<long, AnimationHolder> entry in _animationQueue)
            {
                if (_gameObjects.ContainsKey(entry.Key))
                {
                    GameObject obj = _gameObjects[entry.Key];
                    if (obj != null)
                    {
                        WorldObject worldObject = obj.GetComponent<WorldObject>();
                        if (worldObject != null)
                        {
                            if (worldObject.GetDistance() <= VISIBILITY_RADIUS) // Object is in sight radius.
                            {
                                worldObject.AnimateObject(entry.Value.GetVelocityX(), entry.Value.GetVelocityZ(), entry.Value.IsTriggerJump(), entry.Value.IsInWater(), entry.Value.IsGrounded());
                            }
                        }
                    }
                }

                ((IDictionary<long, AnimationHolder>)_animationQueue).Remove(entry.Key);
            }

            // Update pending characters.
            foreach (KeyValuePair<long, CharacterDataHolder> entry in _characterUpdateQueue)
            {
                if (_gameObjects.ContainsKey(entry.Key))
                {
                    GameObject obj = _gameObjects[entry.Key];
                    if (obj != null)
                    {
                        WorldObject worldObject = obj.GetComponent<WorldObject>();
                        if (worldObject != null)
                        {
                            if (worldObject.GetDistance() <= VISIBILITY_RADIUS) // Object is in sight radius.
                            {
                                DynamicCharacterAvatar avatar = obj.GetComponent<DynamicCharacterAvatar>();
                                if (avatar != null)
                                {
                                    // TODO: Manage more things than just item updates.
                                    CharacterDataHolder oldData = worldObject.GetCharacterData();
                                    CharacterDataHolder newData = entry.Value;

                                    int headItem = newData.GetHeadItem();
                                    if (headItem != oldData.GetHeadItem())
                                    {
                                        if (headItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.HEAD);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, headItem);
                                        }
                                    }

                                    int chestItem = newData.GetChestItem();
                                    if (chestItem != oldData.GetChestItem())
                                    {
                                        if (chestItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.CHEST);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, chestItem);
                                        }
                                    }

                                    int legsItem = newData.GetLegsItem();
                                    if (legsItem != oldData.GetLegsItem())
                                    {
                                        if (legsItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.LEGS);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, legsItem);
                                        }
                                    }

                                    int handsItem = newData.GetHandsItem();
                                    if (handsItem != oldData.GetHandsItem())
                                    {
                                        if (handsItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.HANDS);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, handsItem);
                                        }
                                    }

                                    int feetItem = newData.GetFeetItem();
                                    if (feetItem != oldData.GetFeetItem())
                                    {
                                        if (feetItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.FEET);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, feetItem);
                                        }
                                    }

                                    int leftHandItem = newData.GetLeftHandItem();
                                    if (leftHandItem != oldData.GetLeftHandItem())
                                    {
                                        if (leftHandItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.LEFT_HAND);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, leftHandItem);
                                        }
                                    }

                                    int rightHandItem = newData.GetRightHandItem();
                                    if (rightHandItem != oldData.GetRightHandItem())
                                    {
                                        if (rightHandItem == 0)
                                        {
                                            CharacterManager.Instance.UnEquipItem(avatar, ItemSlot.RIGHT_HAND);
                                        }
                                        else
                                        {
                                            CharacterManager.Instance.EquipItem(avatar, rightHandItem);
                                        }
                                    }

                                    // Update world object with new data.
                                    worldObject.SetCharacterData(newData);
                                }
                            }
                        }
                    }
                }

                ((IDictionary<long, CharacterDataHolder>)_characterUpdateQueue).Remove(entry.Key);
            }
        }
    }

    public DynamicCharacterAvatar GetActiveCharacter()
    {
        return _activeCharacter;
    }

    public WorldObject GetTargetWorldObject()
    {
        return _targetWorldObject;
    }

    public bool IsPlayerInWater()
    {
        return _isPlayerInWater;
    }

    public void SetPlayerInWater(bool value)
    {
        _isPlayerInWater = value;
    }

    public bool IsPlayerOnTheGround()
    {
        return _isPlayerOnTheGround;
    }

    public void SetPlayerOnTheGround(bool value)
    {
        _isPlayerOnTheGround = value;
    }

    public void SetKickFromWorld(bool value)
    {
        _kickFromWorld = value;
    }

    public ConcurrentDictionary<long, GameObject> GetGameObjects()
    {
        return _gameObjects;
    }

    public ConcurrentDictionary<long, MovementHolder> GetMoveQueue()
    {
        return _moveQueue;
    }

    public ConcurrentDictionary<long, AnimationHolder> GetAnimationQueue()
    {
        return _animationQueue;
    }

    // Calculate distance between player and a Vector3 location.
    public double CalculateDistance(Vector3 vector)
    {
        return Math.Pow(MovementController.GetStoredPosition().x - vector.x, 2) + Math.Pow(MovementController.GetStoredPosition().y - vector.y, 2) + Math.Pow(MovementController.GetStoredPosition().z - vector.z, 2);
    }

    public void UpdateObject(long objectId, CharacterDataHolder characterdata)
    {
        lock (UPDATE_OBJECT_LOCK) // Use lock to avoid adding duplicate gameObjects.
        {
            // Check for existing objects.
            if (_gameObjects.ContainsKey(objectId))
            {
                // Update object info.
                ((IDictionary<long, CharacterDataHolder>)_characterUpdateQueue).Remove(objectId);
                _characterUpdateQueue.TryAdd(objectId, characterdata);
                return;
            }

            // Object is out of sight.
            if (CalculateDistance(new Vector3(characterdata.GetX(), characterdata.GetY(), characterdata.GetZ())) > VISIBILITY_RADIUS)
            {
                return;
            }

            // Add placeholder to game object list.
            _gameObjects.GetOrAdd(objectId, (GameObject)null);

            // Queue creation.
            CharacterManager.Instance.GetCharacterCreationQueue().TryAdd(objectId, characterdata);
        }
    }

    private IEnumerator DelayedDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);

        // Destroy name text.
        Destroy(obj.GetComponent<WorldObjectText>().GetNameMesh().gameObject);
        // Finally destroy gameobject.
        Destroy(obj);
    }

    public void DeleteObject(long objectId)
    {
        lock (UPDATE_METHOD_LOCK)
        {
            if (!_deleteQueue.Contains(objectId))
            {
                _deleteQueue.Add(objectId);
            }
        }
    }

    public void ExitWorld()
    {
        _exitingWorld = true;
        if (_activeCharacter != null)
        {
            Destroy(_activeCharacter.GetComponent<WorldObjectText>().GetNameMesh().gameObject);
            Destroy(_activeCharacter.gameObject);
        }
        _isPlayerInWater = false;
        _isPlayerOnTheGround = false;
        foreach (GameObject obj in _gameObjects.Values)
        {
            if (obj == null)
            {
                continue;
            }

            Destroy(obj.GetComponent<WorldObjectText>().GetNameMesh().gameObject);
            Destroy(obj);
        }
        CharacterManager.Instance.GetCharacterCreationQueue().Clear();
    }

    public void SetTarget(WorldObject worldObject)
    {
        // Unset old target.
        if (_targetWorldObject != null)
        {
            WorldObjectText previousObjectText = _targetWorldObject.GetComponentInParent<WorldObjectText>();
            if (previousObjectText != null)
            {
                previousObjectText.SetCurrentColor(WorldObjectText.DEFAULT_COLOR_NPC);
            }
        }

        // Set new target.
        _targetWorldObject = worldObject;
        if (_targetWorldObject != null)
        {
            WorldObjectText worldObjectText = _targetWorldObject.GetComponentInParent<WorldObjectText>();
            if (worldObjectText != null)
            {
                worldObjectText.SetCurrentColor(WorldObjectText.SELECTED_COLOR);
            }

            // Send new target id to server.
            NetworkManager.SendPacket(new TargetUpdateRequest(_targetWorldObject.GetObjectId()));
        }
        else // Target has been unset.
        {
            NetworkManager.SendPacket(new TargetUpdateRequest(-1));
        }

        // Update UI target information.
        StatusInformationManager.Instance.UpdateTargetInformation(_targetWorldObject);
    }
}
