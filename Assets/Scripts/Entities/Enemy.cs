using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public enum MovementStatus
{
    None,
    Knockback,
}

public abstract class Enemy : MonoBehaviour, ISpellTarget, IFreezeTarget
{
    protected float moveSpeed = 1f;
    
    // Components
    protected Rigidbody rb;
    private Renderer _renderer;
    
    // Private Variables
    private float _maxHealth;
    private float _health;
    private Status _status = Status.None;
    private MovementStatus _movementStatus = MovementStatus.None;
    private float _frozenTimer = 0.0f;
    private float _knockbackTimer = 0.0f;
    private MaterialPropertyBlock _propertyBlock;

    public Status Status => _status;

    private bool _activated = false;

    public Action<Enemy> OnRelease;
    public Action<Damage> OnDamage;
    private static readonly int OutlineColour = Shader.PropertyToID("_Outline_Colour");
    private static readonly int OutlineThickness = Shader.PropertyToID("_Outline_Thickness");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new();
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
    
    public Transform GetTransform()
    {
        return transform;
    }

    private void Update()
    {
        if (!_activated)
            return;
        if (_status == Status.Frozen)
        {
            _frozenTimer -= Time.deltaTime;
            if (_frozenTimer <= 0f)
                Unfreeze();
            return;
        }

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

    public void Freeze(float frozenTime)
    {
        _frozenTimer = frozenTime;
        _status = Status.Frozen;
        rb.isKinematic = true;
    }

    public void Unfreeze()
    {
        _status = Status.None;
        rb.isKinematic = false;
    }

    public void Knockback(Vector3 knockbackForce, float knockbackTime)
    {
        _movementStatus = MovementStatus.Knockback;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
        _knockbackTimer = knockbackTime;
    }

    public void Damage(Damage damage)
    {
        _health -= damage.Amount;
        Debug.Log($"Enemy Took Damage {damage}, Remaining Health is {_health}");
        OnDamage?.Invoke(damage);
        if (IsDead)
        {
            OnRelease?.Invoke(this);
        }
    }
    
    public bool IsDead => _health <= 0f;

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
        _propertyBlock.SetColor(OutlineColour, colour);
        _propertyBlock.SetFloat(OutlineThickness, thickness);
        _renderer.SetPropertyBlock(_propertyBlock, 1);
    }

    public void DisableOutline()
    {
        SetOutline(Color.clear, 0f);
    }
}
