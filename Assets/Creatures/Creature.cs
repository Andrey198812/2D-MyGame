using Assets.Script;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] public float _speed = 5f;
    [SerializeField] protected float _jumpSpeed = 10f;
    [SerializeField] protected float _damageJumpSpeed = 5f;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected SpawnListComponent _particles;
    [SerializeField] protected float _radius;
    [SerializeField] protected bool _invertScale;

    [Header("Combat")]
    [SerializeField] protected int _damage;
    [SerializeField] protected CheckCircleOverlap _attackRange;

    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected Vector2 _direction;
    protected bool _isJumping;
    protected bool _wasGrounded;
    protected float _lastTakeDamageTime = 0f;
    protected static readonly int attackKey = Animator.StringToHash("attack");
    protected static readonly int isGroundKey = Animator.StringToHash("is_ground");
    protected static readonly int isRunningKey = Animator.StringToHash("is_running");
    protected static readonly int verticalVelocityKey = Animator.StringToHash("vertical_velocity");
    protected static readonly int hitKey = Animator.StringToHash("hit");

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction) => _direction = direction;

    protected bool IsGrounded()
    {
        Vector2 point = (Vector2)transform.position + Vector2.down * 0.4f;
        float radius = 0.05f;


        Collider2D hit = Physics2D.OverlapCircle(point, radius, _groundLayer);

        if (hit != null)
        {

        }

        return hit != null;
    }

    protected virtual void FixedUpdate()
    {

        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

        var isGrounded = IsGrounded();

        if (isGrounded && !_wasGrounded && _particles != null)
        {
            _particles.Spawn("LandDust");
        }

        if (isGrounded) _isJumping = false;

        _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);
        _animator.SetBool(isRunningKey, _direction.x != 0);

        if (!_isJumping) _animator.SetBool(isGroundKey, isGrounded);

        _wasGrounded = isGrounded;
    }

    public virtual void JumpPressed()
    {
        if (IsGrounded())
        {
            _isJumping = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
            _animator.SetBool(isGroundKey, false);
            _animator.SetFloat(verticalVelocityKey, _jumpSpeed);

            if (_particles != null) _particles.Spawn("JumpDust");
        }
    }

    public void JumpReleased()
    {
        if (_rigidbody.velocity.y > 0)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
    }

    public virtual void takeDamage()
    {

        _animator.SetTrigger(hitKey);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
    }
    public virtual void Attack()
    {
        Debug.Log("Attack() вызван!");
        _animator.SetTrigger(Animator.StringToHash("attack"));
    }
    private void OnDrawGizmos()
    {
        Vector2 point = (Vector2)transform.position + Vector2.down * 0.4f;
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(point, 0.05f);
    }
    public virtual void InAttack(string targetTag)
    {
        if (_attackRange == null) return;


        var gos = _attackRange.GetObjectInRangee();

        foreach (var go in gos)
        {
            var hp = go.GetComponent<HealthComponent>();

            if (hp != null && go.CompareTag(targetTag))
            {
                hp.ApplyDamage(_damage);
                Debug.Log($"{gameObject.name} нанес урон {go.name}");
            }
        }
    }
    public virtual void Die()
    {
        Debug.Log($"Объект {gameObject.name} умер.");


        if (_animator != null)
        {
            _animator.SetTrigger("die");
        }


        StopAllCoroutines();


        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }


        _direction = Vector2.zero;


        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        var attackRange = GetComponentInChildren<CheckCircleOverlap>();
        if (attackRange != null) attackRange.enabled = false;

        var attackCollider = GetComponentInChildren<CircleCollider2D>();
        if (attackCollider != null) attackCollider.enabled = false;


        var allScripts = GetComponents<MonoBehaviour>();
        foreach (var script in allScripts)
        {
            if (script != this && !(script is HealthComponent))
            {
                script.enabled = false;
            }
        }


        this.enabled = false;


        Destroy(gameObject, 2f);
    }

}