using UnityEngine;

public class RunAction : MonoBehaviour, IActorAction
{
    public enum FaceDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private Vector2 _lookDirection = new Vector2(0, 0);
    
    private static readonly int LookX = Animator.StringToHash("Look X");
    private static readonly int LookY = Animator.StringToHash("Look Y");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Agr = Animator.StringToHash("Agr");

    [Header("Settings")]
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    
    [Header("Effects")]
    [SerializeField] private GameObject _runEffectPrefab;
    [SerializeField] private bool _needPlayRunSoundEffect = false;
    [SerializeField] private float _walkSoundRepeatedTime = 1f;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    
    private GameObject _effectObject;
    private AudioSource _effectAudioSource;
    private float _nextSoundEffectPlayTime = 0f;
    
    private Vector2 _desiredVelocity;
    public Vector2 LookDirection => _lookDirection;

    private ActorController _actorController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _actorController = GetComponent<ActorController>();
        _actorController.Register(this);
        _actorController.OnMoveInput += OnMoveInputHandler;
        
        //CreateRunEffect();
    }

    private void OnMoveInputHandler(Vector2 input)
    {
        _desiredVelocity = input;
    
        if (!Mathf.Approximately(input.x, 0.0f) || !Mathf.Approximately(input.y, 0.0f))
        {
            _lookDirection.Set(input.x, input.y);
            _lookDirection.Normalize();
        }
    }

    public void SetSpeed(float speed)
    {
        _maxSpeed = speed;
    }
    
    public float GetSpeed()
    {
        return _maxSpeed;
    }
    
    public FaceDirection GetDirection()
    {
        var direction = FaceDirection.Down;
        
        if (_lookDirection.y > 0)
        {
            direction = FaceDirection.Up;
        } 
        else if (_lookDirection.y < 0)
        {
            direction = FaceDirection.Down;
        }
        else if (_lookDirection.x < 0)
        {
            direction = FaceDirection.Left;
        }
        else if (_lookDirection.x > 0)
        {
            direction = FaceDirection.Right;
        }

        return direction;
    }

    public void Run()
    {
        var velocity = _rigidbody.velocity;
        var maxSpeedChange = _maxAcceleration * Time.deltaTime;

        velocity = Vector2.MoveTowards(velocity, _desiredVelocity * Mathf.Max(_maxSpeed, 0f), maxSpeedChange);

        _rigidbody.velocity = velocity;

        if (_animator != null)
        {
            _animator.SetFloat(LookX, _lookDirection.x);
            _animator.SetFloat(LookY, _lookDirection.y);
            _animator.SetFloat(Speed, velocity.sqrMagnitude);
        }

        // Effect
        //PlayRunEffect(speed > 0);
    }

    private void CreateRunEffect()
    {
        var parentTransform = transform;
            
        _effectObject = Instantiate(_runEffectPrefab, parentTransform.position, Quaternion.identity, parentTransform);
        _effectAudioSource = _effectObject.GetComponent<AudioSource>();
    }

    private void PlayRunEffect(bool isRun)
    {
        if (_needPlayRunSoundEffect)
        {
            var needPlaySoundEffect = Time.time >= _nextSoundEffectPlayTime;

            if (isRun && !_effectAudioSource.isPlaying && needPlaySoundEffect)
            {
                _effectAudioSource.Play();

                _nextSoundEffectPlayTime = Time.time + _walkSoundRepeatedTime;
            }
        }
    }
}