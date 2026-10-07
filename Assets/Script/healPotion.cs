using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int _healAmount;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponent<HealthComponent>();
        if (health == null) return;

        health.Heal(_healAmount);
        if (_animator != null)
        {
            _animator.Play("destroy");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DestroyHealtPotion()
    {
        Destroy(gameObject);
    }
}

