using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 10th 2019
 */
public class MovementController : MonoBehaviour
{
    // Static values.
    private readonly string LAYER_GROUND_VALUE = "Everything";
    private readonly string WATER_TAG_VALUE = "Water";
    // Configs.
    public float _speed = 1.0f;
    public float _speedRotation = 8.0f;
    public float _speedWater = 0.999f;
    public float _speedJump = 5.5f;
    public float _jumpPower = 7.5f;
    public float _distToGround = 0.1f;
    public float _waterLevel = 63.2f;
    // Non-static values.
    private float _speedCurrent = 0;
    private static bool _leftSideMovement = false;
    private static bool _rightSideMovement = false;
    private static bool _lockedMovement = false;
    private static float _storedRotation = 0;
    private static Vector3 _storedPosition = Vector3.zero;
    private Rigidbody _rigidBody;
    private LayerMask _layerGround;

    private void Start()
    {
        _layerGround = LayerMask.NameToLayer(LAYER_GROUND_VALUE);
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = !WorldManager.Instance.IsPlayerInWater();
        _storedPosition = transform.position;
        _storedRotation = transform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        // Set player grounded state.
        WorldManager.Instance.SetPlayerOnTheGround(Physics.Raycast(_rigidBody.transform.position, Vector3.down, _distToGround, _layerGround));

        // Calculate current speed.
        _speedCurrent = WorldManager.Instance.IsPlayerInWater() ? _speedWater : WorldManager.Instance.IsPlayerOnTheGround() ? _speed : _speedJump;

        // Do nothing when chat is active.
        if (MainManager.Instance.IsChatBoxActive())
        {
            if (_lockedMovement)
            {
                transform.localPosition += transform.forward * _speedCurrent * Time.deltaTime;
            }
        }
        else
        {
            // Check for locked movement.
            if (InputManager.NUMLOCK_DOWN || InputManager.SIDE_MOUSE_DOWN || (!_lockedMovement && InputManager.LEFT_MOUSE_PRESS && InputManager.RIGHT_MOUSE_PRESS) || (_lockedMovement && (InputManager.RIGHT_MOUSE_UP || InputManager.UP_PRESS || InputManager.DOWN_PRESS)))
            {
                _lockedMovement = !_lockedMovement;
            }

            // Jump.
            if (InputManager.SPACE_PRESS)
            {
                if (WorldManager.Instance.IsPlayerInWater())
                {
                    // TODO: Check if player goes upper than water level.
                    // if (Physics.Raycast(rBody.transform.position, Vector3.up, distToGround, layerWater))
                    if (transform.position.y <= _waterLevel)
                    {
                        transform.localPosition += transform.up * _speedCurrent * Time.deltaTime;
                    }
                }
                else if (WorldManager.Instance.IsPlayerOnTheGround())
                {
                    _speedCurrent = _speedJump;
                    _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _jumpPower, _rigidBody.velocity.y);
                }
            }

            // Front.
            if (InputManager.UP_PRESS || _lockedMovement)
            {
                transform.localPosition += transform.forward * _speedCurrent * Time.deltaTime;
            }

            // Back.
            if (InputManager.DOWN_PRESS)
            {
                transform.localPosition -= transform.forward * (_speedCurrent * 0.66f) * Time.deltaTime;
            }

            // Check for side movement.
            if (!InputManager.RIGHT_MOUSE_PRESS)
            {
                _leftSideMovement = false;
                _rightSideMovement = false;
            }

            // Left.
            if (InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS)
            {
                if (InputManager.RIGHT_MOUSE_PRESS && !_lockedMovement && !InputManager.UP_PRESS && !InputManager.DOWN_PRESS && !InputManager.LEFT_MOUSE_PRESS)
                {
                    if (!_leftSideMovement)
                    {
                        _leftSideMovement = true;
                        SetPlayerRotation(CameraController.Instance.transform.rotation.eulerAngles.y - 90);
                    }
                    transform.localPosition += transform.forward * _speedCurrent * Time.deltaTime;
                }
                else if (!_leftSideMovement)
                {
                    _rightSideMovement = false;
                    if (InputManager.LEFT_MOUSE_PRESS || (!(InputManager.LEFT_MOUSE_PRESS && !InputManager.DOWN_PRESS)))
                    {
                        SetPlayerRotation(transform.rotation.eulerAngles.y - (!InputManager.LEFT_MOUSE_PRESS ? _speedRotation : _speedRotation * 0.66f));
                    }
                    else
                    {
                        SetPlayerRotationWithLerp(CameraController.Instance.transform.rotation.eulerAngles.y);
                    }
                }
            }
            else
            {
                _leftSideMovement = false;
            }

            // Right.
            if (InputManager.RIGHT_PRESS && !InputManager.LEFT_PRESS)
            {
                if (InputManager.RIGHT_MOUSE_PRESS && !_lockedMovement && !InputManager.UP_PRESS && !InputManager.DOWN_PRESS && !InputManager.LEFT_MOUSE_PRESS)
                {
                    if (!_rightSideMovement)
                    {
                        _rightSideMovement = true;
                        SetPlayerRotation(CameraController.Instance.transform.rotation.eulerAngles.y + 90);
                    }
                    transform.localPosition += transform.forward * _speedCurrent * Time.deltaTime;
                }
                else if (!_rightSideMovement)
                {
                    _leftSideMovement = false;
                    if (InputManager.LEFT_MOUSE_PRESS || (!(InputManager.LEFT_MOUSE_PRESS && !InputManager.DOWN_PRESS)))
                    {
                        SetPlayerRotation(transform.rotation.eulerAngles.y + (!InputManager.LEFT_MOUSE_PRESS ? _speedRotation : _speedRotation * 0.66f));
                    }
                    else
                    {
                        SetPlayerRotationWithLerp(CameraController.Instance.transform.rotation.eulerAngles.y);
                    }
                }
            }
            else
            {
                _rightSideMovement = false;
            }
        }

        // Send changes to network.
        if (_storedRotation != transform.localRotation.eulerAngles.y
            || _storedPosition.x != transform.position.x //
            || _storedPosition.y != transform.position.y //
            || _storedPosition.z != transform.position.z)
        {
            NetworkManager.SendPacket(new LocationUpdateRequest(transform.position.x, transform.position.y, transform.position.z, transform.localRotation.eulerAngles.y));
            _storedPosition = transform.position;
            _storedRotation = transform.localRotation.eulerAngles.y;
        }
    }

    private void SetPlayerRotation(float newRotation)
    {
        Quaternion curHeading = transform.localRotation;
        Vector3 curvAngle = curHeading.eulerAngles;
        curvAngle.y = newRotation;
        curHeading.eulerAngles = curvAngle;
        transform.localRotation = curHeading;
    }

    private void SetPlayerRotationWithLerp(float newRotation)
    {
        Quaternion oldHeading = transform.localRotation;
        Quaternion newHeading = transform.localRotation;
        Vector3 curvAngle = newHeading.eulerAngles;
        curvAngle.y = newRotation;
        newHeading.eulerAngles = curvAngle;
        transform.localRotation = Quaternion.Lerp(oldHeading, newHeading, Time.deltaTime * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(WATER_TAG_VALUE) && !WorldManager.Instance.IsPlayerInWater())
        {
            WorldManager.Instance.SetPlayerInWater(true);
            _rigidBody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(WATER_TAG_VALUE) && WorldManager.Instance.IsPlayerInWater())
        {
            WorldManager.Instance.SetPlayerInWater(false);
            _rigidBody.useGravity = true;
        }
    }

    public static bool IsLeftSideMovement()
    {
        return _leftSideMovement;
    }

    public static bool IsRightSideMovement()
    {
        return _rightSideMovement;
    }

    public static float GetStoredRotation()
    {
        return _storedRotation;
    }

    public static Vector3 GetStoredPosition()
    {
        return _storedPosition;
    }
}
