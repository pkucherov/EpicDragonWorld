using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 11th 2018
 */
public class WorldObject : MonoBehaviour
{
    private long _objectId;
    private volatile CharacterDataHolder _characterData;
    private double _distance = 0;

    private Animator _animator;
    private Rigidbody _rigidBody;

    // Is grounded related.
    private volatile bool _isGrounded = false;

    // Is in water related.
    private volatile bool _isInWater = false;

    // Sound related.
    private AudioSource _audioSource;
    private static readonly float SOUND_DISTANCE = 1000;

    private void Start()
    {
        _distance = WorldManager.Instance.CalculateDistance(transform.position);
        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = true;
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void MoveObject(Vector3 newPosition, float heading)
    {
        if (gameObject == null || !gameObject.activeSelf)
        {
            return;
        }

        float step = Time.deltaTime * 10;
        _rigidBody.MovePosition(Vector3.Lerp(transform.position, newPosition, step));

        Quaternion oldHeading = transform.localRotation;
        Quaternion newHeading = transform.localRotation;
        Vector3 curvAngle = newHeading.eulerAngles;
        curvAngle.y = heading;
        newHeading.eulerAngles = curvAngle;
        transform.localRotation = Quaternion.Lerp(oldHeading, newHeading, step);

        // Update distance value.
        _distance = WorldManager.Instance.CalculateDistance(transform.position);

        // Set audioSource volume based on distance.
        _audioSource.volume = (1 - (float)(_distance / SOUND_DISTANCE) * OptionsManager.Instance.GetSfxVolume());

        // Animation related sounds.
        if (_distance < SOUND_DISTANCE)
        {
            // Movement footstep sounds.
            if (!_audioSource.isPlaying && _rigidBody.velocity.magnitude > 2 && _isGrounded)
            {
                _audioSource.PlayOneShot(SoundManager.Instance.FOOTSTEP_SOUND, 1);
            }
        }
    }

    public void AnimateObject(float velocityX, float velocityZ, bool triggerJump, bool isInWater, bool isGrounded)
    {
        if (gameObject == null || !gameObject.activeSelf)
        {
            return;
        }

        _isGrounded = isGrounded;
        _isInWater = isInWater;
        _rigidBody.useGravity = !isInWater;

        _animator.SetBool(AnimationController.IS_GROUNDED_VALUE, isGrounded);
        _animator.SetBool(AnimationController.IS_IN_WATER_VALUE, isInWater);
        _animator.SetFloat(AnimationController.VELOCITY_Z_VALUE, velocityZ);
        _animator.SetFloat(AnimationController.VELOCITY_X_VALUE, velocityX);
        if (triggerJump)
        {
            _animator.SetTrigger(AnimationController.TRIGGER_JUMP_VALUE);
        }
    }

    public long GetObjectId()
    {
        return _objectId;
    }

    public void SetObjectId(long value)
    {
        _objectId = value;
    }

    public CharacterDataHolder GetCharacterData()
    {
        return _characterData;
    }

    public void SetCharacterData(CharacterDataHolder value)
    {
        _characterData = value;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public bool IsInWater()
    {
        return _isInWater;
    }

    public double GetDistance()
    {
        return _distance;
    }
}