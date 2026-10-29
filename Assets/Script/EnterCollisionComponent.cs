using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private UnityEvent<GameObject> _action;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Шип столкнулся с: {collision.gameObject.name}, тег: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag(_tag))
        {
            _action?.Invoke(collision.gameObject);
        }
    }
}
