using UnityEngine;
using UnityEngine.Events;
using System;

public class HealthComponent : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onHeal;
    [SerializeField] private HealthChangeEvent _onChange;

    [Header("Settings")]
    [SerializeField] private int _maxHp = 5;

    private int _health; 
    private bool _isDead;
    private float _lastDamageTime = 0f;
    private float _damageCooldown = 0.5f;

    private void Start()
    {
        _isDead = false;
        _health = _maxHp;
        _onChange?.Invoke(GetCurrentHealth());
    }

    public void ApplyDamage(int damageValue)
    {
        if (_isDead) return;
        if (gameObject.CompareTag("Player"))
        {
            if (Time.time - _lastDamageTime < _damageCooldown) return;
        }
        _lastDamageTime = Time.time;

        var session = FindObjectOfType<GameSession>();

        if (gameObject.CompareTag("Player")) 
        {
            if (session != null)
            {
                session.Data.Hp -= damageValue;
                if (session.Data.Hp < 0) session.Data.Hp = 0;

                _onDamage?.Invoke();
                _onChange?.Invoke(session.Data.Hp);

                if (session.Data.Hp <= 0)
                {
                    _isDead = true;
                    _onDie?.Invoke();
                }
            }
        }
        else 
        {
            _health -= damageValue;
            _onDamage?.Invoke();
            _onChange?.Invoke(_health);

            if (_health <= 0)
            {
                _isDead = true;
                _onDie?.Invoke();
            }
        }
    }

    public void Heal(int healValue)
    {
        if (_isDead) return;
        var session = FindObjectOfType<GameSession>();
        if (session != null && gameObject.CompareTag("Player"))
        {
            session.Data.Hp += healValue;
            if (session.Data.Hp > _maxHp) session.Data.Hp = _maxHp;
            _onHeal?.Invoke();
            _onChange?.Invoke(session.Data.Hp);
        }
    }

    public void SetHealth(int health)
    {
        var session = FindObjectOfType<GameSession>();
        if (session != null && gameObject.CompareTag("Player"))
            session.Data.Hp = health;
    }

    private int GetCurrentHealth()
    {
        var session = FindObjectOfType<GameSession>();
        if (session != null && gameObject.CompareTag("Player"))
            return session.Data.Hp;
        return _health; 
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int> { }
}