using UMA.CharacterSystem;
using UnityEngine;

/**
 * Author: Paintbrush
 * Date: July 24th 2018
 */
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public float _targetHeight = 1.7f;
    public float _offsetFromWall = 0.1f;
    public float _maxDistance = 10;
    public float _minDistance = 1;
    public float _speedDistance = 5;
    public float _xSpeed = 200.0f;
    public float _ySpeed = 200.0f;
    public float _rotationDampening = 3.0f;
    public float _zoomDampening = 5.0f;
    public int _yMinLimit = -40;
    public int _yMaxLimit = 80;
    public int _zoomRate = 40;
    public LayerMask _collisionLayers = -1;

    private Transform _target;
    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private float _currentDistance = 5f;
    private float _desiredDistance = 5f;
    private float _correctedDistance = 5f;

    private void Start()
    {
        Instance = this;

        // Make the rigid body not change rotation.
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Camera logic on LateUpdate to only update after all character movement logic has been handled.
    private void LateUpdate()
    {
        // Don't do anything if target is not defined.
        if (_target == null)
        {
            DynamicCharacterAvatar activeCharacter = WorldManager.Instance.GetActiveCharacter();
            if (activeCharacter != null)
            {
                // Now we can set target.
                _target = activeCharacter.transform;

                // Bring camera behing player.
                _xDeg = _target.eulerAngles.y;
                _yDeg = 10;
            }
            return;
        }

        // If either mouse buttons are down, let the mouse govern camera position.
        if (GUIUtility.hotControl == 0 && !MainManager.Instance.IsDraggingWindow())
        {
            if (InputManager.LEFT_MOUSE_PRESS || (InputManager.RIGHT_MOUSE_PRESS && !InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS) || MovementController.IsRightSideMovement() || MovementController.IsLeftSideMovement())
            {
                _xDeg += InputManager.AXIS_MOUSE_X * _xSpeed * 0.02f;
                _yDeg -= InputManager.AXIS_MOUSE_Y * _ySpeed * 0.02f;

                if (!InputManager.LEFT_MOUSE_PRESS || InputManager.RIGHT_MOUSE_PRESS)
                {
                    if (!InputManager.LEFT_PRESS && !InputManager.RIGHT_PRESS)
                    {
                        SetPlayerRotation(transform.rotation.eulerAngles.y);
                    }

                    // Hide cursor while rotating.
                    Cursor.visible = false;
                }

                // Hide cursor while rotating.
                if (InputManager.UP_PRESS || InputManager.DOWN_PRESS || InputManager.LEFT_PRESS || InputManager.RIGHT_PRESS)
                {
                    Cursor.visible = false;
                }
            }
            // Otherwise, ease behind the target if any of the directional keys are pressed and chat is not active.
            else if (!MainManager.Instance.IsChatBoxActive() && (InputManager.LEFT_PRESS || InputManager.RIGHT_PRESS))
            {
                float targetRotationAngle = _target.eulerAngles.y;
                float currentRotationAngle = transform.eulerAngles.y;
                _xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, _rotationDampening * Time.deltaTime);
            }
        }

        // When in water, move towards camera direction, if right mouse button is pressed and player is not rotating the camera.
        if (WorldManager.Instance.IsPlayerInWater() && InputManager.RIGHT_MOUSE_PRESS && (InputManager.LEFT_MOUSE_PRESS || InputManager.UP_PRESS || InputManager.DOWN_PRESS))
        {
            _target.position += transform.forward * Time.deltaTime;
        }

        // Calculate the desired distance.
        if (!MainManager.Instance.IsChatBoxActive() && !MainManager.Instance.IsDraggingWindow()) // Do not want to intervene with chat scrolling.
        {
            _desiredDistance -= InputManager.AXIS_MOUSE_SCROLLWHEEL * Time.deltaTime * _zoomRate * Mathf.Abs(_desiredDistance) * _speedDistance;
        }
        _desiredDistance = Mathf.Clamp(_desiredDistance, _minDistance, _maxDistance);

        _yDeg = ClampAngle(_yDeg, _yMinLimit, _yMaxLimit);

        // Det camera rotation.
        Quaternion rotation = Quaternion.Euler(_yDeg, _xDeg, 0);
        _correctedDistance = _desiredDistance;

        // Calculate desired camera position.
        Vector3 vTargetOffset = new Vector3(0, -_targetHeight, 0);
        Vector3 position = _target.position - (rotation * Vector3.forward * _desiredDistance + vTargetOffset);

        // Check for collision using the true target's desired registration point as set by user using height.
        Vector3 trueTargetPosition = new Vector3(_target.position.x, _target.position.y, _target.position.z) - vTargetOffset;

        // If there was a collision, correct the camera position and calculate the corrected distance.
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out RaycastHit collisionHit, _collisionLayers.value))
        {
            // Calculate the distance from the original estimated position to the collision location,
            // subtracting out a safety "offset" distance from the object we hit.  The offset will help
            // keep the camera from being right on top of the surface we hit, which usually shows up as
            // the surface geometry getting partially clipped by the camera's front clipping plane.
            _correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - _offsetFromWall;
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance.
        _currentDistance = !isCorrected || _correctedDistance > _currentDistance ? Mathf.Lerp(_currentDistance, _correctedDistance, Time.deltaTime * _zoomDampening) : _correctedDistance;

        // Keep within legal limits.
        _currentDistance = Mathf.Clamp(_currentDistance, _minDistance, _maxDistance);

        // Recalculate position based on the new currentDistance.
        position = _target.position - (rotation * Vector3.forward * _currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

    private void SetPlayerRotation(float newRotation)
    {
        Quaternion oldHeading = _target.localRotation;
        Quaternion newHeading = _target.localRotation;
        Vector3 curvAngle = newHeading.eulerAngles;
        curvAngle.y = newRotation;
        newHeading.eulerAngles = curvAngle;
        _target.localRotation = Quaternion.Lerp(oldHeading, newHeading, Time.deltaTime * 10); // 10 is response time.
    }
}