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
    }

    protected override void Move()
    {
    }

    protected override void PlayAttackAnimation()
    {
        animator.SetTrigger(RangedAttack);
    }
    
    protected override void Attack()
    {
        Debug.Log("Enemy Archer Attack");
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
