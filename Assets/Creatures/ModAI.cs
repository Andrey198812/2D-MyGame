using System.Collections;
using UnityEngine;

public class ModAI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private LayerCheck _canAttack;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _alertObject;
    [SerializeField] private Animator _alertAnimator;

    [Header("Settings")]
    [SerializeField] private float _alarmDelay = 0.5f;
    [SerializeField] private float _attackCooldown = 1f;

    private Coroutine _current;
    private GameObject _target;
    private Creature _creature;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;
    private Patrol _patrol;

    private bool _isAgro = false;

    private void Awake()
    {
        _creature = GetComponent<Creature>();
        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _patrol = GetComponent<Patrol>();
    }

    private void Start()
    {
       
        StartState(_patrol.DoPatrol());
    }

    public void OnHeroInvision()
    {
        if (_isAgro) return;

        Debug.Log("EnterTrigger: Акула увидела игрока!");
        _target = GameObject.FindGameObjectWithTag("Player");

        if (_target == null)
        {
            Debug.LogError("Акула не может найти игрока! Поставь тег 'Player' на Героя.");
            return;
        }

        _isAgro = true;
        StartState(AgroToHero());
    }

    private IEnumerator AgroToHero()
    {
        _creature.SetDirection(Vector2.zero);

        if (_alertObject != null)
        {
            _alertObject.SetActive(true);
        }

        yield return new WaitForSeconds(_alarmDelay);

        if (_alertObject != null)
        {
            _alertObject.SetActive(false);
        }

       
        StartState(GoToHero());
    }

    private IEnumerator GoToHero()
    {
        while (_vision != null && _vision.IsTouchingLayer)
        {
            if (_canAttack != null && _canAttack.IsTouchingLayer)
            {
                StartState(Attack());
                yield break;
            }
            else
            {
                SetDirectionToTarget();
            }
            yield return null;
        }

        Debug.Log("GoToHero: Потерял игрока из виду. Возвращаюсь в патруль.");
        _creature.SetDirection(Vector2.zero);
        _isAgro = false;

       
        StartState(_patrol.DoPatrol());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            bool canAttack = _canAttack != null && _canAttack.IsTouchingLayer;

            if (canAttack)
            {
                _creature.SetDirection(Vector2.zero);
                _creature.Attack(); 

               
                yield return new WaitForSeconds(0.3f);

               
                if (!this.enabled)
                {
                    yield break;
                }

               
                _creature.InAttack("Player");

               
                yield return new WaitForSeconds(_attackCooldown - 0.3f);
            }
            else
            {
                break;
            }
            yield return null;
        }

        
        StartState(GoToHero());
    }

    private void SetDirectionToTarget()
    {
        if (_target != null)
        {
            Vector2 direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }
    }

    private void StartState(IEnumerator coroutine)
    {
        if (_current != null) StopCoroutine(_current);
        _current = StartCoroutine(coroutine);
    }

    private void Update()
    {
        if (_rb != null && Mathf.Abs(_rb.velocity.x) > 0.1f)
        {
            if (_rb.velocity.x > 0)
                _sprite.flipX = true;
            else if (_rb.velocity.x < 0)
                _sprite.flipX = false;
        }
    }
}