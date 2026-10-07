using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.05f;
    [SerializeField] private float _damageJumpSpeed = 5f;
    [SerializeField] private Collider2D[] _interactionresult = new Collider2D[1];
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _minRunTimeForDust = 0.15f;
    // [SerializeField] private SpawnComponent _footStepParticles;
    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private Animator _footStepAnimator;
    [SerializeField] private Transform _footStepParticlesTransform;
    [SerializeField] private GameObject _landDustPrefab;
    [SerializeField] private Transform _landSpawnPoint;
    [SerializeField] private Transform _jumpSpawnPoint;
    [SerializeField] private GameObject _justDustPrefab;
    [SerializeField] private float _minFallSpeedForDust = 3f;

    private float _runTimer;
    private float _fallSpeedAtLanding;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _isJumping;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private bool _wasGrounded;
    private  PlayerScore _score;
    private int coins;
    
    private static readonly int isGroundKey = Animator.StringToHash("is_ground");
    private static readonly int isRunningKey = Animator.StringToHash("is_running");
    private static readonly int verticalVelocityKey = Animator.StringToHash("vertical_velocity");
    private static readonly int hitKey = Animator.StringToHash("hit");

    private void Awake()
    {
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _score = GetComponent<PlayerScore>();
        coins = _score.TotalCoins;
    }

    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void JumpPressed()
    {
        if (IsGrounded())
        {
            _isJumping = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);

            _animator.SetBool(isGroundKey, false);
            _animator.SetFloat(verticalVelocityKey, _jumpSpeed);
            Instantiate(_justDustPrefab, _jumpSpawnPoint.position, Quaternion.identity);

        }
    }

    public void JumpReleased()
    {
        if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

        var isGrounded = IsGrounded();
        if (!isGrounded)
        {
            _fallSpeedAtLanding = _rigidbody.velocity.y;
        }
        if (isGrounded && !_wasGrounded)
        {
            if (_fallSpeedAtLanding < -_minFallSpeedForDust)
            {
                Instantiate(_landDustPrefab, _landSpawnPoint.position, Quaternion.identity);
            }
        }
        if (isGrounded)
        {
            _isJumping = false;
        }
        

        _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
        _animator.SetBool(isRunningKey, _direction.x != 0);

        if (!_isJumping)
        {
            _animator.SetBool(isGroundKey, isGrounded);
        }

        UpdateSpriteDirection();
        HandleFootSteps(isGrounded);

        _wasGrounded = isGrounded;
    }
    private void HandleFootSteps(bool isGrounded)
    {
        bool isMoving = Mathf.Abs(_direction.x) > 0.1f;

        if (isGrounded && isMoving)
        {
            _runTimer += Time.fixedDeltaTime;

            
            if (_runTimer >= _minRunTimeForDust)
            {
                if (!_footStepAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    _footStepAnimator.Play("Run");
                }
            }
        }
        else
        {
            _runTimer = 0f;   

            if (!_footStepAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _footStepAnimator.Play("Idle");
            }
        }
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _sprite.flipX = false;
            _footStepParticlesTransform.localPosition = new Vector3(-0.365f, -0.062f, 0);
            _footStepParticlesTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
            _footStepParticlesTransform.localPosition = new Vector3(0.365f, -0.062f, 0);
            _footStepParticlesTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private bool IsGrounded()
    {
        Vector2 point = (Vector2)transform.position + Vector2.down * 0.4f;
        float radius = 0.05f;

        Collider2D hit = Physics2D.OverlapCircle(point, radius, _groundLayer);
        return hit != null;
    }

    private void OnDrawGizmos()
    {
        Vector2 point = (Vector2)transform.position + Vector2.down * 0.4f;
        float radius = 0.05f;

        Gizmos.color = IsGrounded() ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(point, radius);
    }


    public void takeDamage()
    {
        
        _animator.SetTrigger(hitKey);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
        coins = _score.TotalCoins;
        if (coins > 0) 
        {
            SpawnCoins();
        }
    }

    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(coins, 5);
        _score.RemoveCoins(numCoinsToDispose);
        coins -= numCoinsToDispose;
        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsToDispose;
        _hitParticles.emission.SetBurst(0, burst);
        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
    }

    public void Interact()
    {
        Debug.Log("Hero.Interact вызван");
        float radius = 1f;
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, _interactionresult, _interactionLayer);
        Debug.Log($"Найдено объектов: {size}");

        for (int i = 0; i < size; i++)
        {
            Debug.Log($"Объект {i}: {_interactionresult[i].gameObject.name}, слой: {LayerMask.LayerToName(_interactionresult[i].gameObject.layer)}");

            var interactable = _interactionresult[i].GetComponent<InteractableComponent>();
            Debug.Log($"InteractableComponent: {(interactable == null ? "НЕТ" : "есть")}");

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

}