using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Snowball : SingleTargetSpell, IProjectileSpell
{
    private float _freezeTime;
    private float _launchSpeed;
    private float _knockbackForce;
    private float _knockbackTime; 
    private Projectile _snowballPrefab;

    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        SnowballSpellConfig snowballConfig = config as SnowballSpellConfig;
        if (snowballConfig == null)
            throw new InvalidCastException("Config must be of type SnowballSpellConfig.");
        _freezeTime = snowballConfig.freezeTime;
        _launchSpeed = snowballConfig.launchSpeed;
        _knockbackForce = snowballConfig.knockbackForce;
        _knockbackTime = snowballConfig.knockbackTime;
        _snowballPrefab = snowballConfig.projectilePrefab;
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return Frozen.CanAffect(enemy);
    }

    protected override void Apply(Enemy spellTarget)
    {
        // Init the fireball
        Transform launchPosition = ServiceLocator.Instance.Get<IGameManager>().GetPlayer().GetLaunchPosition();
        Projectile snowball = Instantiate(_snowballPrefab);
        snowball.Init(this);
        snowball.transform.SetPositionAndRotation(launchPosition.position, launchPosition.rotation);
        
        // Launch the fireball
        Vector3 targetPosition = spellTarget.GetTransform().position;
        Vector3 launchDirection = targetPosition - snowball.transform.position;
        Vector3 launchForce = launchDirection * _launchSpeed;
        snowball.Rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }
    
    public void ApplyEnemy(Enemy enemy, Projectile projectile)
    {
        // The burn will get rid of frozen
        Frozen frozen = new (Status.Frozen, _freezeTime);
        Damage spellDamage = new (damage, DamageType.Ice, DamageEffect.None);
        
        enemy.ApplyStatus(frozen);
        enemy.Damage(spellDamage);

        // Knockback the enemy only if they die from the fireball
        if (enemy.IsDead)
        {
            Vector3 direction = projectile.Rigidbody.linearVelocity;
            enemy.Knockback(direction.normalized * _knockbackForce, _knockbackTime);
        }
        
        // Destroy the fireball projectile
        Destroy(projectile.gameObject);
    }
}
