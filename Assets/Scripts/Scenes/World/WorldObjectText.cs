using TMPro;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: February 9th 2019
 */
public class WorldObjectText : MonoBehaviour
{
    public static Color32 DEFAULT_COLOR = new Color32(0, 255, 0, 255);
    public static Color32 SELECTED_COLOR = new Color32(0, 128, 128, 255);

    private GameObject _attachedObject;
    private WorldObject _worldObject;
    private TextMeshPro _nameMesh;
    private string _worldObjectName = "";
    private Color32 _currentColor = DEFAULT_COLOR;

    private float _currentHeight;
    private int _raceId;
    private bool _isInWater;

    private void Start()
    {
        GameObject newGameObject = new GameObject();

        _nameMesh = newGameObject.AddComponent<TextMeshPro>();
        _nameMesh.color = _currentColor;
        _nameMesh.text = "";
        _nameMesh.alignment = TextAlignmentOptions.Center;
        _nameMesh.fontSize = 1.5f;
    }

    private void LateUpdate()
    {
        if (_attachedObject == null || gameObject == null || !_attachedObject.activeSelf || !gameObject.activeSelf)
        {
            return;
        }

        // Reset information in case of unknown or changed race.
        _currentHeight = 1f;
        if (_worldObject == null)
        {
            _raceId = MainManager.Instance.GetSelectedCharacterData().GetRace();
            _isInWater = WorldManager.Instance.IsPlayerInWater();
        }
        else
        {
            _raceId = _worldObject.GetCharacterData().GetRace();
            _isInWater = _worldObject.IsInWater();
        }

        // Height based on race.
        switch (_raceId)
        {
            case 0:
                _currentHeight = 1f;
                break;

            case 1:
                _currentHeight = 0.85f;
                break;
        }

        // When in water, reduce height.
        if (_isInWater)
        {
            _currentHeight -= 0.3f;
        }

        _nameMesh.color = _currentColor;
        _nameMesh.text = _worldObjectName;
        _nameMesh.transform.position = new Vector3(_attachedObject.transform.position.x, _attachedObject.transform.position.y + _attachedObject.transform.lossyScale.y + _currentHeight, _attachedObject.transform.position.z);
        _nameMesh.transform.LookAt(CameraController.Instance.transform.position);
        _nameMesh.transform.Rotate(0, 180, 0);
    }

    public void SetAttachedObject(GameObject value)
    {
        _attachedObject = value;
    }

    public void SetWorldObject(WorldObject value)
    {
        _worldObject = value;
    }

    public TextMeshPro GetNameMesh()
    {
        return _nameMesh;
    }

    public void SetWorldObjectName(string value)
    {
        _worldObjectName = value;
    }

    public void SetCurrentColor(Color32 value)
    {
        _currentColor = value;
    }
}
