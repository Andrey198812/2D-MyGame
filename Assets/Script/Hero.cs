using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 1.2f;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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

        if (IsGrounded())
        {
            _isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        float distance = _groundCheckDistance;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, _groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        float radius = 0.3f; 

        Gizmos.color = IsGrounded() ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(origin, radius);
    }
}