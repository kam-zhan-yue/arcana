using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EnemyArcher : Enemy, IProjectileEnemy
{
    [SerializeField] private Crossbow crossbow;
    
    private static readonly int RangedAttack = Animator.StringToHash("RangedAttack");
    private static readonly int AimStart = Animator.StringToHash("AimStart");
    private float _startupTime;
    private ProjectileEnemy _arrowPrefab;
    private float _arrowSpeed;
    private bool _startup = false;
    private float _startupTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        crossbow.gameObject.SetActiveFast(false);
    }

    protected override void OnInit(EnemyData data)
    {
        base.OnInit(data);
        crossbow.gameObject.SetActiveFast(true);
        animator.SetTrigger(AimStart);
        EnemyArcherConfig config = data.config as EnemyArcherConfig;
        if (config == null)
            throw new InvalidCastException("Config must be of type EnemyArcherConfig");
        _startupTime = config.startupTime;
        _arrowPrefab = config.arrowPrefab;
        _arrowSpeed = config.arrowSpeed;
        _startup = true;
    }

    protected override void Move()
    {
    }

    protected override void AttackUpdate()
    {
        if (_startup)
        {
            _startupTimer += Time.deltaTime;
            if (_startupTimer >= _startupTime)
                _startup = true;
            return;
        }
        
        base.AttackUpdate();
    }

    protected override void Attack()
    {
        Debug.Log("Enemy Archer Attack");
        animator.SetTrigger(RangedAttack);
        Player player = GetPlayer();

        ProjectileEnemy projectile = Instantiate(_arrowPrefab);
        projectile.Init(this, player, crossbow.launchPoint, _arrowSpeed);
    }

    public void ApplyPlayer(Player player, ProjectileEnemy projectile)
    {
        player.Damage();
        Destroy(projectile.gameObject);
    }
}
