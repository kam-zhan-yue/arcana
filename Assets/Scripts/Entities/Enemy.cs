using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public enum MovementStatus
{
    None,
    Attacking,
    Spawning,
    Knockback,
}

public enum AttackState
{
    None,
    Waiting,
    Animating,
    Attack,
}

public abstract class Enemy : MonoBehaviour
{
    private const float KNOCKBACK_TIME = 0.2f;
    [SerializeField] private float height = 0.5f;
    
    private const int OUTLINE_MATERIAL_INDEX = 1;
    private const int PULSE_MATERIAL_INDEX = 2;
    private const int FROZEN_MATERIAL_INDEX = 3;
    protected float moveSpeed = 1f;
    
    // Components
    public Rigidbody Rigidbody => rb;
    protected Rigidbody rb;
    protected Animator animator;
    private Renderer[] _renderers = Array.Empty<Renderer>();
    
    // Private Variables
    private float _maxHealth;
    private float _health;
    protected float attackRange;
    protected float timeBetweenAttacks;
    private MovementStatus _movementState = MovementStatus.None;
    private float _knockbackTimer = 0.0f;
    private MaterialPropertyBlock _outlinePropertyBlock;
    private MaterialPropertyBlock _pulsePropertyBlock;
    private MaterialPropertyBlock _frozenPropertyBlock;
    private float _attackAnimationTime;
    private AttackState _attackState = AttackState.None;
    
    public Status Status { get; private set; } = Status.None;
    private StatusEffect _statusEffect;
    public StatusEffect StatusEffect => _statusEffect;

    private bool _inited = false;
    private float _attackTimer = 0f;

    public Action<Enemy> OnRelease;
    public Action<Damage> OnDamage;
    public Action<Status> OnStatus;
    private static readonly int OutlineColour = Shader.PropertyToID("_Outline_Colour");
    private static readonly int OutlineThickness = Shader.PropertyToID("_Outline_Thickness");
    private static readonly int PulseColour = Shader.PropertyToID("_Pulse_Colour");
    private static readonly int PulseAmount = Shader.PropertyToID("_Pulse_Amount");
    private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
    private static readonly int SpawnGround = Animator.StringToHash("SpawnGround");
    private static readonly int Dead = Animator.StringToHash("Dead");

    public bool IsVulnerable => !IsDead && _movementState != MovementStatus.Spawning;
    private bool _startup = false;
    private float _startupTimer = 0f;
    private float _startupTime = 0f;
    private static readonly int IsFrozen = Shader.PropertyToID("_IsFrozen");

    private float _pulseTimer = 0f;
    private float _pulseTime = 0f;
    private Color _pulseColour;
    private Color _frozenColour;
    private float _frozenAlpha;
    private AnimationCurve _pulseCurve;
    private Light _light;
    private float _lightIntensity;
    private float _lightTweenDuration;
    private Tween _lightTween;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _renderers = GetComponentsInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        _light = GetComponentInChildren<Light>();
        _outlinePropertyBlock = new MaterialPropertyBlock();
        _pulsePropertyBlock = new MaterialPropertyBlock();
        _frozenPropertyBlock = new MaterialPropertyBlock();
    }

    public Vector3 GetCenter()
    {
        Vector3 center = new(transform.position.x, transform.position.y + height, transform.position.z);
        return center;
    }

    protected Player GetPlayer()
    {
        Game game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        return game.Player;
    }

    protected void FacePlayer()
    {
        Player player = GetPlayer();
        Vector3 direction = (player.transform.position - rb.position).normalized;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation( direction.normalized, Vector3.up);
            rb.MoveRotation(lookRotation);
        }
    }

    public void Init(EnemyData data)
    {
        foreach (Renderer rend in _renderers)
        {
            List<Material> materials = new List<Material>();
            rend.GetMaterials(materials);
            materials.Add(data.outlineShader);
            materials.Add(data.pulseShader);
            rend.SetMaterials(materials);
        }
        FacePlayer();
        GameDatabase database = ServiceLocator.Instance.Get<IGameManager>().GetGame().Database;
        GameSettings gameSettings = database.settings;
        _frozenColour = gameSettings.frozenColour;
        _frozenAlpha = gameSettings.frozenAlpha;
        _pulseCurve = gameSettings.pulseCurve;
        _lightIntensity = database.enemyDatabase.lightIntensity;
        _lightTweenDuration = database.enemyDatabase.lightTweenDuration;
        EnableLight();
        
        if (data.spawnFromGround)
        {
            SpawnFromGround(data).Forget();
        }
        else
        {
            OnInit(data);
        }
    }

    private async UniTask SpawnFromGround(EnemyData data)
    {
        _movementState = MovementStatus.Spawning;
        animator.SetTrigger(SpawnGround);
        await UniTask.WaitForSeconds(data.timeToSpawn);
        _movementState = MovementStatus.None;
        OnInit(data);
    }


    protected virtual void OnInit(EnemyData data)
    {
        _attackAnimationTime = data.config.attackAnimationTime;
        moveSpeed = data.config.moveSpeed;
        attackRange = data.config.attackRange;
        timeBetweenAttacks = data.config.timeBetweenAttacks;
        _maxHealth = data.config.maxHealth;
        _health = _maxHealth;
        _startupTime = data.config.startupTime;
        _startupTimer = 0f;
        _startup = true;
        
        DisablePulse();
        
        _inited = true;
    }

    private void Update()
    {
        if (!_inited)
            return;
        
        UpdateStatus();
        UpdatePulse();
        if (!IsDead && Status == Status.Frozen)
        {
            animator.speed = 0f;
        }
        else
        {
            animator.speed = 1f;
            animator.SetFloat(WalkSpeed, Rigidbody.linearVelocity.magnitude);
        }

        if (_startup)
        {
            _startupTimer += Time.deltaTime;
            if (_startupTimer >= _startupTime)
                _startup = false;
        }
        
        switch (_movementState)
        {
            case MovementStatus.None:
                if (CanMove())
                    MoveUpdate();
                break;
            case MovementStatus.Attacking:
                if (CanMove())
                    AttackUpdate();
                break;
            case MovementStatus.Knockback:
                _knockbackTimer -= Time.deltaTime;
                if (_knockbackTimer <= 0f)
                {
                    ResetVelocity();
                    _movementState = MovementStatus.None;
                }
                break;
        }
    }

    protected virtual void MoveUpdate()
    {
        Move();
        if (DistanceToPlayer() <= attackRange)
        {
            ResetVelocity();
            _movementState = MovementStatus.Attacking;
            _attackState = AttackState.None;
        }
    }

    protected abstract void PlayAttackAnimation();

    private void AttackUpdate()
    {
        _attackTimer += Time.deltaTime;
        switch (_attackState)
        {
            case AttackState.None:
                _attackState = AttackState.Waiting;
                _attackTimer = 0f;
                break;
            case AttackState.Waiting:
                if (_attackTimer >= timeBetweenAttacks)
                {
                    PlayAttackAnimation();
                    _attackTimer -= timeBetweenAttacks;
                    _attackState = AttackState.Animating;
                }
                break;
            case AttackState.Animating:
                if (_attackTimer >= _attackAnimationTime)
                {
                    _attackTimer -= _attackAnimationTime;
                    _attackState = AttackState.Attack;
                }
                break;
            case AttackState.Attack:
                Attack();
                _attackState = AttackState.None;
                break;
        }
    }

    private void ResetVelocity()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected virtual bool CanAttack()
    {
        return true;
    }

    private bool CanMove()
    {
        return Status != Status.Frozen && !IsDead && !_startup;
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(GetPlayer().transform.position, transform.position);
    }

    protected abstract void Move();

    private void Knockback(Vector3 knockbackForce, float knockbackTime)
    {
        ResetVelocity();
        _movementState = MovementStatus.Knockback;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
        _knockbackTimer = knockbackTime;
    }

    public void Damage(Damage damage)
    {
        // If already dead, don't do anything
        if (IsDead) return;
        _health -= damage.Amount;
        OnDamage?.Invoke(damage);
        
        GameSettings settings = ServiceLocator.Instance.Get<IGameManager>().GetGame().Database.settings;
        TypeSetting typeSetting = settings.GetSettingByType(damage.Type);
        if (typeSetting != null)
        {
            _pulseTimer = 0f;
            _pulseTime = settings.pulseTime;
            _pulseColour = typeSetting.flashColour;
        }
        
        if (IsDead)
        {
            Die();
        }
        else if(damage.KnockbackForce > 0f)
        {
            Game game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
            Player player = game.Player;
            Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
            Knockback(knockbackDirection * damage.KnockbackForce, KNOCKBACK_TIME);
        }
    }

    public void Mutate(GameObject mutation)
    {
        GameObject mutate = Instantiate(mutation);
        Vector3 newPosition = transform.position;
        newPosition.y += height;
        mutate.transform.SetPositionAndRotation(newPosition, transform.rotation);
        Die(true);
    }

    private void Die(bool immediatelyDestroy = false)
    {
        if (_statusEffect != null)
            _statusEffect.Complete(this);
        DisableOutline();
        DisablePulse();
        DisableLight();
        ResetVelocity();
        animator.SetTrigger(Dead);
        OnRelease?.Invoke(this);
        if(immediatelyDestroy)
            Destroy(gameObject);
        else
            DieAsync().Forget();
    }

    private async UniTask DieAsync()
    {
        await UniTask.WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    public bool IsDead => _health <= 0f;

    private void UpdateStatus()
    {
        if (_statusEffect != null)
        {
            _statusEffect.Update(this, Time.deltaTime);
            if (_statusEffect.completed)
            {
                SetStatus(Status.None);
                _statusEffect = null;
            }

        }
    }

    private void UpdatePulse()
    {
        if (_pulseTimer <= _pulseTime)
        {
            _pulseTimer += Time.deltaTime;
            float value = Mathf.Clamp01(_pulseTimer / _pulseTime);
            float pulseAmount = _pulseCurve.Evaluate(value);
            SetPulseMaterial(_pulseColour, pulseAmount);
        }
        else if (_statusEffect != null && _statusEffect.status == Status.Frozen)
        {
            SetPulseMaterial(_frozenColour, _frozenAlpha);
        }
        else
        {
            DisablePulse();
        }
    }
    
    public void ApplyStatus(StatusEffect statusEffect)
    {
        bool applied = statusEffect.Apply(this);
        if (applied)
        {

            _pulseTimer = 0;
            // Complete the previous status effect if it was set
            _statusEffect?.Complete(this);
            _statusEffect = statusEffect;
            SetStatus(statusEffect.status);
        }
    }
    
    private void SetStatus(Status newStatus)
    {
        if (Status != newStatus)
        {
            Status = newStatus;
            OnStatus?.Invoke(Status);
        }
    }

    protected abstract void Attack();

    public void SetOutline(Color colour, float thickness)
    {
        _outlinePropertyBlock.SetColor(OutlineColour, colour);
        _outlinePropertyBlock.SetFloat(OutlineThickness, thickness);
        foreach (Renderer rend in _renderers)
            rend.SetPropertyBlock(_outlinePropertyBlock, OUTLINE_MATERIAL_INDEX);
    }

    public void DisableOutline()
    {
        SetOutline(Color.clear, 0f);
    }

    private void SetPulseMaterial(Color colour, float amount)
    {
        _pulsePropertyBlock.SetColor(PulseColour, colour);
        _pulsePropertyBlock.SetFloat(PulseAmount, amount);
        foreach (Renderer rend in _renderers)
            rend.SetPropertyBlock(_pulsePropertyBlock, PULSE_MATERIAL_INDEX);
    }

    public void EnableLight()
    {
        _lightTween?.Kill(); // Cancel any existing tween
        _light.enabled = true;
        _lightTween = DOTween.To(() => _light.intensity, x => _light.intensity = x, _lightIntensity, _lightTweenDuration);
    }

    public void DisableLight()
    {
        _lightTween?.Kill(); // Cancel any existing tween
        _lightTween = DOTween.To(() => _light.intensity, x => _light.intensity = x, 0f, _lightTweenDuration)
            .OnComplete(() => _light.enabled = false);
    }
    
    private void DisablePulse()
    {
        SetPulseMaterial(Color.clear, 0f);
    }
    
    private void OnDestroy()
    {
        _lightTween?.Kill();
    }
}
