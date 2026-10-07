using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] private int _maxHp = 5;

    private bool _isDead;

    public void ApplyDamage(int damageValue)
    {
        _health -= damageValue;
        _onDamage?.Invoke();

        if(_health <= 0)
        {
            _onDie?.Invoke();
        }
    }

    public void Heal(int healValue)
    {
        if (_isDead) return;

        _health += healValue;
        if (_health > _maxHp)
            _health = _maxHp;

        _onHeal?.Invoke();
    

    Debug.Log($"Вылечено {healValue} очков здоровья! Здоровье: {_health}");



    }
}
