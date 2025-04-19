using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Fireball : SingleTargetSpell, IProjectileSpell
{
    private float _burnTime;
    private float _burnDamage;
    private float _burnTick;
    private float _launchSpeed;
    private float _knockbackForce;
    private float _knockbackTime; 
    private Projectile _fireballPrefab;

    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        FireballSpellConfig fireballConfig = config as FireballSpellConfig;
        if (fireballConfig == null)
            throw new InvalidCastException("Config must be of type FireballSpellConfig.");
        _burnTime = fireballConfig.burnTime;
        _burnDamage = fireballConfig.burnDamage;
        _burnTick = fireballConfig.burnTick;
        _launchSpeed = fireballConfig.launchSpeed;
        _knockbackForce = fireballConfig.knockbackForce;
        _knockbackTime = fireballConfig.knockbackTime;
        _fireballPrefab = fireballConfig.projectilePrefab;
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return Burn.CanAffect(enemy);
    }

    protected override void Apply(Enemy spellTarget)
    {
        // Init the fireball
        Transform launchPosition = ServiceLocator.Instance.Get<IGameManager>().GetPlayer().GetLaunchPosition();
        Projectile fireball = Instantiate(_fireballPrefab);
        fireball.Init(this);
        fireball.transform.SetPositionAndRotation(launchPosition.position, launchPosition.rotation);
        
        // Launch the fireball
        Vector3 targetPosition = spellTarget.GetTransform().position;
        Vector3 launchDirection = targetPosition - fireball.transform.position;
        Vector3 launchForce = launchDirection * _launchSpeed;
        fireball.Rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }
    

    public void ApplyEnemy(Enemy enemy, Projectile projectile)
    {
        float multiplier = 1f;
        
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Frozen)
        {
            effect = DamageEffect.Melt;
        }
        
        // The burn will get rid of frozen
        Burn burn = new (Status.Burned, _burnTime, _burnDamage, _burnTick);
        Damage fireballDamage = new (damage, DamageType.Fire, effect);
        
        enemy.ApplyStatus(burn);
        enemy.Damage(fireballDamage);

        // Knockback the enemy only if they die from the fireball
        if (enemy.IsDead)
        {
            Vector3 direction = projectile.Rigidbody.linearVelocity;
            enemy.Knockback(direction.normalized * _knockbackForce * multiplier, _knockbackTime);
        }
        
        // Destroy the fireball projectile
        Destroy(projectile.gameObject);
    }
}
