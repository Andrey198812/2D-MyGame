using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector2 _direction;


    [SerializeField] private float _speed;

   public void SetDurection(Vector2 direction)
    {
        _direction = direction;
    }
    private void Update()
    {
        if(_direction != Vector2.zero)
        {
            var deltaX = _direction.x * _speed * Time.deltaTime;
            var deltaY = _direction.y * _speed * Time.deltaTime;
            var newXPosition = transform.position.x + deltaX;
            var newYPosition = transform.position.y + deltaY;
            transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);
        }
    }
}
