using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public enum MovementStatus
{
    None,
    Knockback,
}

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private float height = 0.5f;
    
    private const int OUTLINE_MATERIAL_INDEX = 1;
    private const int PULSE_MATERIAL_INDEX = 2;
    protected float moveSpeed = 1f;
    
    // Components
    public Rigidbody Rigidbody => rb;
    protected Rigidbody rb;
    private Renderer[] _renderers = Array.Empty<Renderer>();
    
    // Private Variables
    private float _maxHealth;
    private float _health;
    private MovementStatus _movementStatus = MovementStatus.None;
    private float _knockbackTimer = 0.0f;
    private MaterialPropertyBlock _outlinePropertyBlock;
    private MaterialPropertyBlock _pulsePropertyBlock;
    
    public Status Status { get; private set; } = Status.None;
    private StatusEffect _statusEffect;
    public StatusEffect StatusEffect => _statusEffect;

    private bool _activated = false;

    public Action<Enemy> OnRelease;
    public Action<Damage> OnDamage;
    public Action<Status> OnStatus;
    private static readonly int OutlineColour = Shader.PropertyToID("_Outline_Colour");
    private static readonly int OutlineThickness = Shader.PropertyToID("_Outline_Thickness");
    private static readonly int PulseColour = Shader.PropertyToID("_Pulse_Colour");
    private static readonly int PulseAmount = Shader.PropertyToID("_Pulse_Amount");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _renderers = GetComponentsInChildren<Renderer>();
        _outlinePropertyBlock = new();
        _pulsePropertyBlock = new();
    }

    public Vector3 GetCenter()
    {
        Vector3 center = new(transform.position.x, transform.position.y + height, transform.position.z);
        return center;
    }

    public void Init(EnemyConfig config)
    {
        moveSpeed = config.moveSpeed;
        _maxHealth = config.maxHealth;
        _health = _maxHealth;
    }

    public void Activate()
    {
        _activated = true;
    }

    private void Update()
    {
        if (!_activated)
            return;
        UpdateStatus();
        switch (_movementStatus)
        {
            case MovementStatus.None:
                Move();
                break;
            case MovementStatus.Knockback:
                _knockbackTimer -= Time.deltaTime;
                if (_knockbackTimer <= 0f)
                    _movementStatus = MovementStatus.None;
                break;
        }
    }

    protected abstract void Move();

    public void Knockback(Vector3 knockbackForce, float knockbackTime)
    {
        _movementStatus = MovementStatus.Knockback;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
        _knockbackTimer = knockbackTime;
    }

    public void Damage(Damage damage)
    {
        _health -= damage.Amount;
        OnDamage?.Invoke(damage);
        if (IsDead)
        {
            OnRelease?.Invoke(this);
        }
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
            }
        }
    }
    
    public void ApplyStatus(StatusEffect statusEffect)
    {
        bool applied = statusEffect.Apply(this);
        if (applied)
        {
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

    public void Attack()
    {
        if (ServiceLocator.Instance)
        {
            Player player = ServiceLocator.Instance.Get<IGameManager>().GetPlayer();
            Debug.Log("Attack!");
        }
        
    }

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

    public void SetPulse(Color colour, float amount)
    {
        _pulsePropertyBlock.SetColor(PulseColour, colour);
        _pulsePropertyBlock.SetFloat(PulseAmount, amount);
        foreach (Renderer rend in _renderers)
            rend.SetPropertyBlock(_pulsePropertyBlock, PULSE_MATERIAL_INDEX);
    }

    public void DisablePulse()
    {
        SetPulse(Color.clear, 0f);
    }
}
