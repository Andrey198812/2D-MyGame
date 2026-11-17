using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody;
    private int _direction;

    private void Awake()
    {
       
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
        var hero = FindObjectOfType<Hero>();
        if (hero != null)
        {
            var sr = hero.GetComponent<SpriteRenderer>();
            _direction = sr.flipX ? -1 : 1;
        }
        else
        {
            _direction = transform.localScale.x > 0 ? 1 : -1;
        }
    }

    private void FixedUpdate()
    {
        if (_rigidbody == null) return; 

        var position = _rigidbody.position;
        position.x += _direction * _speed;
        _rigidbody.MovePosition(position);
    }
}