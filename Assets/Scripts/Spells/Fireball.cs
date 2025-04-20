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

    public (Vector3 position, Quaternion rotation) GetLaunchPlatform()
    {
        Transform launch = ServiceLocator.Instance.Get<IGameManager>().GetGame().Player.GetLaunchPosition();
        return (launch.position, launch.rotation);
    }

    protected override void Apply(Enemy spellTarget)
    {
        // Init the fireball
        Projectile projectile = Instantiate(_fireballPrefab);
        projectile.Init(this, spellTarget, _launchSpeed);
    }

    public void ApplyEnemy(Enemy enemy, Projectile projectile)
    {
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Frozen)
        {
            effect = DamageEffect.Melt;
        }
        
        // The burn will get rid of frozen
        Burn burn = new (Status.Burned, _burnTime, _burnDamage, _burnTick);
        Damage fireballDamage = new (damage, DamageType.Fire, effect, _knockbackForce);
        
        enemy.ApplyStatus(burn);
        enemy.Damage(fireballDamage);
        
        // Destroy the fireball projectile
        Destroy(projectile.gameObject);
    }
}
