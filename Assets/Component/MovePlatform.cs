using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed;

    private Vector2 _target;
    [SerializeField] private bool _isMoving = false;


    private void Start()
    {
        _target = _pointB.position;
    }
    private void Update()
        
    {
        if (_isMoving) return;
        transform.position = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, _target) < 0.01f)
        {
            _target = _target == (Vector2)_pointB.position ? _pointA.position : _pointB.position;
        }
    }

    public void SetMoving(bool value)
    {
        _isMoving = value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

}
