using UnityEngine;
using UnityEngine.InputSystem;


public class Barrel : MonoBehaviour
{
    [SerializeField] float force = 3f;
    [SerializeField] float friction = 0.95f; 

    private Rigidbody2D _rigidbody;
    private bool _onGround;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Vector2 dir = (transform.position - col.transform.position).normalized;
            _rigidbody.AddForce(dir * force, ForceMode2D.Force);
        }

        if (col.gameObject.tag == "Ground")
        {
            _onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            _onGround = false;
        }
    }

    void FixedUpdate()
    {
        if (_onGround)
        {
            Vector2 vel = _rigidbody.velocity;
            vel.x *= friction;
            _rigidbody.velocity = vel;
        }
    }
}