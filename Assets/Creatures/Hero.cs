using Assets.Script;
using System.Collections;
using UnityEngine;

public class Hero : Creature
{
    [Header("Hero Particles")]
    [SerializeField] private Animator _footStepAnimator;
    [SerializeField] private Transform _footStepParticlesTransform;
    [SerializeField] private float _minRunTimeForDust = 0.15f;
    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private SpawnComponent _swordSpawnComponent;
    [SerializeField] private Transform _throwPosition;

    [Header("Hero Specifics")]
    [SerializeField] private Collider2D[] _interactionresult = new Collider2D[1];
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private RuntimeAnimatorController _armed;
    [SerializeField] private RuntimeAnimatorController _disarmed;
    [SerializeField] private float _coolDownThrow = 1f; // Кулдаун между сериями бросков
    [SerializeField] private float _holdTimeToCharge = 1f; // Сколько нужно держать для залпа
    [SerializeField] private float _burstInterval = 0.2f; // Интервал между мечами в залпе

    private float _runTimer;
    private SpriteRenderer _sprite;
    private GameSession _gameSession;
    private float _lastThrowTime = 0;

    // Переменные для броска
    private bool _isHoldingThrow = false;
    private float _throwHoldTimer = 0f;

    protected static readonly int throwKey = Animator.StringToHash("throw");

    protected override void Awake()
    {
        base.Awake();
        _sprite = GetComponent<SpriteRenderer>();
        _gameSession = FindObjectOfType<GameSession>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        var health = GetComponent<HealthComponent>();
        if (health != null && _gameSession != null) health.SetHealth(_gameSession.Data.Hp);
        UpDateHeroWeapon();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateSpriteDirection();
        HandleFootSteps(IsGrounded());
    }

    private void Update()
    {
        // Считаем время удержания кнопки
        if (_isHoldingThrow)
        {
            _throwHoldTimer += Time.deltaTime;
        }
    }

    private void HandleFootSteps(bool isGrounded)
    {
        bool isMoving = Mathf.Abs(_direction.x) > 0.1f;

        if (isGrounded && isMoving)
        {
            _runTimer += Time.fixedDeltaTime;
            if (_runTimer >= _minRunTimeForDust)
            {
                if (!_footStepAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                    _footStepAnimator.Play("Run");
            }
        }
        else
        {
            _runTimer = 0f;
            if (!_footStepAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                _footStepAnimator.Play("Idle");
        }
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _sprite.flipX = false;

            if (_attackPosition != null)
                _attackPosition.localPosition = new Vector3(0.8f, _attackPosition.localPosition.y, 0);

            if (_throwPosition != null)
                _throwPosition.localPosition = new Vector3(0.8f, _throwPosition.localPosition.y, 0);

            if (_footStepParticlesTransform != null)
            {
                _footStepParticlesTransform.localPosition = new Vector3(-0.365f, -0.062f, 0);
                _footStepParticlesTransform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;

            if (_attackPosition != null)
                _attackPosition.localPosition = new Vector3(-0.8f, _attackPosition.localPosition.y, 0);

            if (_throwPosition != null)
                _throwPosition.localPosition = new Vector3(-0.8f, _throwPosition.localPosition.y, 0);

            if (_footStepParticlesTransform != null)
            {
                _footStepParticlesTransform.localPosition = new Vector3(0.365f, -0.062f, 0);
                _footStepParticlesTransform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public override void takeDamage()
    {
        if (Time.time - _lastTakeDamageTime < 0.5f) return;
        _lastTakeDamageTime = Time.time;

        base.takeDamage();
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        if (_gameSession == null || _hitParticles == null) return;

        int randomCoins = Random.Range(1, 6);
        var numCoinsToDispose = Mathf.Min(_gameSession.Data.Coin, randomCoins);

        if (numCoinsToDispose <= 0) return;

        _gameSession.Data.Coin -= numCoinsToDispose;

        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsToDispose;
        _hitParticles.emission.SetBurst(0, burst);

        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
    }

    public void Interact()
    {
        float radius = 1f;
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, _interactionresult, _interactionLayer);
        for (int i = 0; i < size; i++)
        {
            var interactable = _interactionresult[i].GetComponent<InteractableComponent>();
            if (interactable != null) interactable.Interact();
        }
    }

    public override void Attack()
    {
        if (_animator.runtimeAnimatorController == _disarmed) return;
        base.Attack();
    }

    public void InAttack()
    {
        base.InAttack("Enemy");
    }

    public void ArmHero()
    {
        if (_gameSession == null)
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        if (_gameSession != null && _gameSession.Data != null)
        {
            _gameSession.Data.SwordAmount++;
            _gameSession.Data.IsArmed = true;
            Debug.Log($"Подобрал меч! Теперь их: {_gameSession.Data.SwordAmount}");
            UpDateHeroWeapon();
        }
        else
        {
            Debug.LogError("GameSession или Data не найдены! Не могу подобрать меч.");
        }
    }

    private void UpDateHeroWeapon()
    {
        if (_gameSession == null || _animator == null) return;
        _animator.runtimeAnimatorController = _gameSession.Data.IsArmed ? _armed : _disarmed;
    }


    public void StartThrow()
    {
        if (_isHoldingThrow) return;
        if (Time.time - _lastThrowTime < _coolDownThrow) return;
        if (_gameSession.Data.SwordAmount <= 1) return;

        _isHoldingThrow = true;
        _throwHoldTimer = 0f;
    }


    public void StopThrow()
    {
        if (!_isHoldingThrow) return;

        _isHoldingThrow = false;
        _lastThrowTime = Time.time;


        if (_throwHoldTimer >= _holdTimeToCharge)
        {
            Debug.Log("Заряжено! Бросаем очередью.");
            StartCoroutine(ThrowBurst(3));
        }
        else
        {
            Debug.Log("Быстрый клик. Бросаем один.");
            StartCoroutine(ThrowBurst(1));
        }
    }


    private IEnumerator ThrowBurst(int count)
    {
        int fired = 0;


        while (fired < count && _gameSession.Data.SwordAmount > 1)
        {
            _gameSession.Data.SwordAmount--;
            _animator.SetTrigger(throwKey);
            OnDoThrow();
            fired++;

            Debug.Log($"Брошен меч. Осталось: {_gameSession.Data.SwordAmount}");

            yield return new WaitForSeconds(_burstInterval);
        }
    }

    public void OnDoThrow()
    {
        if (_swordSpawnComponent != null)
        {
            _swordSpawnComponent.Spawn();
        }
    }
}