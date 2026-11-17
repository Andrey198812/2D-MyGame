using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTuchComponent : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var healt = collision.GetComponent<HealthComponent>();
            if (healt != null)
            {
                healt.ApplyDamage(_damage);
                Debug.Log("Меч нанес урон!");

            }
        }
    }
}
