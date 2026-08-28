using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.05f;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _isJumping;
    private Animator _animator;
    private SpriteRenderer _sprite;

    
    private static readonly int isGroundKey = Animator.StringToHash("is_ground");
    private static readonly int isRunningKey = Animator.StringToHash("is_running");
    private static readonly int verticalVelocityKey = Animator.StringToHash("vertical_velocity"); 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
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
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
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
}