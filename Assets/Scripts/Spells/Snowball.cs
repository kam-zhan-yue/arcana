using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Snowball : SingleTargetSpell, IProjectileSpell
{
    private float _freezeTime;
    private float _launchSpeed;
    private float _knockbackForce;
    private float _knockbackTime; 
    private Projectile _snowballPrefab;
    private ParticleSystem _explosionEffect;

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
        _explosionEffect = snowballConfig.explosionEffect;
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return Frozen.CanAffect(enemy);
    }
    public (Vector3 position, Quaternion rotation) GetLaunchPlatform()
    {
        Transform launch = ServiceLocator.Instance.Get<IGameManager>().GetGame().Player.GetLaunchPosition();
        return (launch.position, launch.rotation);
    }

    protected override void Apply(Enemy spellTarget)
    {
        // Init the fireball
        Projectile projectile = Instantiate(_snowballPrefab);
        projectile.Init(this, spellTarget, _launchSpeed);
    }

    public void ApplyEnemy(Enemy enemy, Projectile projectile)
    {
        // The burn will get rid of frozen
        Frozen frozen = new (Status.Frozen, _freezeTime);
        Damage spellDamage = new (damage, DamageType.Ice, DamageEffect.None, _knockbackForce);
        
        enemy.ApplyStatus(frozen);
        enemy.Damage(spellDamage);
        
        // Spawn the explosion effect
        ParticleSystem explosion = Instantiate(_explosionEffect);
        explosion.transform.position = enemy.GetCenter();
        explosion.Play(true);
        
        // Destroy the fireball projectile
        Destroy(projectile.gameObject);
    }
}
