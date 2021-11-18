using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: January 15th 2019
 */
public class AnimationController : MonoBehaviour
{
    // Static values.
    private readonly float MAX_VELOCITY = 10;
    private readonly float VELOCITY_STEP = 2;
    public readonly static string VELOCITY_X_VALUE = "VelocityX";
    public readonly static string VELOCITY_Z_VALUE = "VelocityZ";
    public readonly static string IS_GROUNDED_VALUE = "IsGrounded";
    public readonly static string IS_IN_WATER_VALUE = "IsInWater";
    public readonly static string TRIGGER_JUMP_VALUE = "TriggerJump";
    // Non-static values.
    private Animator _animator;
    private AudioSource _audioSource;
    private float _currentVelocityX;
    private float _currentVelocityZ;
    private bool _lockedMovement = false;
    private bool _sideMovement = false;
    // Values for comparison.
    private float _previousVelocityX = 0;
    private float _previousVelocityZ = 0;
    private bool _triggerJump = false;
    private bool _lastWaterState = false;
    private bool _lastGroundedState = false;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _animator.applyRootMotion = true;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        // Update values.
        _animator.SetBool(IS_GROUNDED_VALUE, WorldManager.Instance.IsPlayerOnTheGround());
        _animator.SetBool(IS_IN_WATER_VALUE, WorldManager.Instance.IsPlayerInWater());

        // Do nothing when chat is active.
        if (MainManager.Instance.IsChatBoxActive())
        {
            if (_lockedMovement)
            {
                _currentVelocityZ = Mathf.Min(MAX_VELOCITY, _currentVelocityZ + VELOCITY_STEP);
            }
            else
            {
                _currentVelocityZ = 0;
            }
            _currentVelocityX = 0;
            _animator.SetFloat(VELOCITY_Z_VALUE, _currentVelocityZ);
            _animator.SetFloat(VELOCITY_X_VALUE, _currentVelocityX);
        }
        else
        {
            // Update values.
            _currentVelocityX = _animator.GetFloat(VELOCITY_X_VALUE);
            _currentVelocityZ = _animator.GetFloat(VELOCITY_Z_VALUE);

            // Check for locked movement.
            if (InputManager.NUMLOCK_DOWN || InputManager.SIDE_MOUSE_DOWN || (!_lockedMovement && InputManager.LEFT_MOUSE_PRESS && InputManager.RIGHT_MOUSE_PRESS) || (_lockedMovement && (InputManager.RIGHT_MOUSE_UP || InputManager.UP_PRESS || InputManager.DOWN_PRESS)))
            {
                _lockedMovement = !_lockedMovement;
            }

            // Check for side movement.
            _sideMovement = InputManager.RIGHT_MOUSE_PRESS && !InputManager.UP_PRESS && !InputManager.DOWN_PRESS && (InputManager.LEFT_PRESS || InputManager.RIGHT_PRESS);

            // Jump.
            if (InputManager.SPACE_PRESS)
            {
                if (WorldManager.Instance.IsPlayerInWater())
                {
                    // Nothing planned for now.
                }
                else if (WorldManager.Instance.IsPlayerOnTheGround())
                {
                    _animator.SetTrigger(TRIGGER_JUMP_VALUE);
                    _triggerJump = true;
                }
            }

            // Front.
            if (InputManager.UP_PRESS || _lockedMovement || _sideMovement)
            {
                _currentVelocityZ = Mathf.Min(MAX_VELOCITY, _currentVelocityZ + VELOCITY_STEP);
            }
            // Back.
            else if (InputManager.DOWN_PRESS)
            {
                _currentVelocityZ = Mathf.Max(-MAX_VELOCITY, _currentVelocityZ - VELOCITY_STEP);
            }
            else // Reduce VelocityZ speed.
            {
                if (_currentVelocityZ > 0)
                {
                    _currentVelocityZ -= VELOCITY_STEP;
                }
                else if (_currentVelocityZ < 0)
                {
                    _currentVelocityZ += VELOCITY_STEP;
                }
            }
            // Set VelocityZ value.
            _animator.SetFloat(VELOCITY_Z_VALUE, _currentVelocityZ);

            // Left.
            if (InputManager.LEFT_PRESS && (InputManager.UP_PRESS || InputManager.DOWN_PRESS))
            {
                _currentVelocityX = Mathf.Max(MAX_VELOCITY, _currentVelocityX - VELOCITY_STEP);
            }
            // Right.
            else if (InputManager.RIGHT_PRESS && (InputManager.UP_PRESS || InputManager.DOWN_PRESS))
            {
                _currentVelocityX = Mathf.Min(-MAX_VELOCITY, _currentVelocityX + VELOCITY_STEP);
            }
            else // Reduce VelocityX speed.
            {
                if (_currentVelocityX > 0)
                {
                    _currentVelocityX -= VELOCITY_STEP;
                }
                else if (_currentVelocityX < 0)
                {
                    _currentVelocityX += VELOCITY_STEP;
                }
            }
            // Set VelocityX value.
            _animator.SetFloat(VELOCITY_X_VALUE, _currentVelocityX);
        }

        // Send changes to network.
        if (_previousVelocityX != _currentVelocityX //
            || _previousVelocityZ != _currentVelocityZ //
            || _triggerJump //
            || _lastWaterState != WorldManager.Instance.IsPlayerInWater() //
            || _lastGroundedState != WorldManager.Instance.IsPlayerOnTheGround())
        {
            NetworkManager.SendPacket(new AnimatorUpdateRequest(_currentVelocityX, _currentVelocityZ, _triggerJump, WorldManager.Instance.IsPlayerInWater(), WorldManager.Instance.IsPlayerOnTheGround()));
            // Store last sent values.
            _previousVelocityX = _currentVelocityX;
            _previousVelocityZ = _currentVelocityZ;
            _triggerJump = false;
            _lastWaterState = WorldManager.Instance.IsPlayerInWater();
            _lastGroundedState = WorldManager.Instance.IsPlayerOnTheGround();
        }
    }

    // Triggered by animation run_fwd at frames 7 and 16.
    private void StepSound()
    {
        _audioSource.volume = OptionsManager.Instance.GetSfxVolume();
        _audioSource.PlayOneShot(SoundManager.Instance.FOOTSTEP_SOUND, 1);
    }
}
