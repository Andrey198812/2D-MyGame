using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static System.Collections.Specialized.BitVector32;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] private HealthChangeEvent _onChange;
    [SerializeField] private int _maxHp = 5;

    private bool _isDead;
    private int CurrentHealth
    {
        get
        {
            var session = FindObjectOfType<GameSession>();
            if (session != null) return session.Data.Hp;
            return 0;
        }
        set
        {
            var session = FindObjectOfType<GameSession>();
            if (session != null) session.Data.Hp = value;
        }
    }

    public void ApplyDamage(int damageValue)
    {
        Debug.Log($"1. Пришел урон: {damageValue}. _isDead = {_isDead}");
        if (_isDead) return;

        if (gameObject.CompareTag("Player"))
        {
            var session = FindObjectOfType<GameSession>();
            if (session != null)
            {
                Debug.Log($"2. HP ДО: {session.Data.Hp}");
                session.Data.Hp -= damageValue;
                if (session.Data.Hp < 0) session.Data.Hp = 0;
                Debug.Log($"3. HP ПОСЛЕ: {session.Data.Hp}");

                if (session.Data.Hp <= 0) 
                {
                    Debug.Log("4. СМЕРТЬ! Вызываю OnDie");
                    _isDead = true; _onDie?.Invoke();
                    return;
                }
            }
        }
        else
        {
            _health -= damageValue;
            if (_health <= 0) { _isDead = true; _onDie?.Invoke();
                return;
            }
        }

        _onDamage?.Invoke();
        return;
    }

    public void Heal(int healValue)
    {
        if (_isDead) return;

        CurrentHealth += healValue;
        if (CurrentHealth > _maxHp)
            CurrentHealth = _maxHp;

        _onHeal?.Invoke();
        _onChange?.Invoke(CurrentHealth);
     


        Debug.Log($"Вылечено {healValue} очков здоровья! Здоровье: {_health}");
    }

    public void SetHealth(int health)
    {
        CurrentHealth = health;
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {

    }
}
