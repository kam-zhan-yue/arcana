using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EnemyMage : Enemy, IProjectileEnemy
{
    [SerializeField] private Transform staff;
    [SerializeField] private Transform launchPoint;
    private static readonly int MagicAttack = Animator.StringToHash("MagicAttack");
    private ProjectileEnemy _spellPrefab;
    private float _spellSpeed;

    protected override void Awake()
    {
        base.Awake();
        staff.gameObject.SetActiveFast(false);
    }

    protected override void OnInit(EnemyData data)
    {
        base.OnInit(data);
        staff.gameObject.SetActiveFast(true);
        EnemyMageConfig config = data.config as EnemyMageConfig;
        if (config == null)
            throw new InvalidCastException("Config must be of type EnemyMageConfig");
        _spellPrefab = config.spellPrefab;
        _spellSpeed = config.spellSpeed;
    }

    protected override void Move()
    {
    }

    protected override void PlayAttackAnimation()
    {
        Debug.Log("Enemy Mage Attack Animation");
        animator.SetTrigger(MagicAttack);
    }

    protected override void Attack()
    {
        Debug.Log("Enemy Mage Attack");
        Player player = GetPlayer();

        ProjectileEnemy projectile = Instantiate(_spellPrefab);
        projectile.Init(this, player, launchPoint, _spellSpeed);
    }

    public void ApplyPlayer(Player player, ProjectileEnemy projectile)
    {
        player.Damage();
        Destroy(projectile.gameObject);
    }
}