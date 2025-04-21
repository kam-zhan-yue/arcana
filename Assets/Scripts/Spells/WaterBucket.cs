using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class WaterBucket : SingleTargetSpell, IProjectileSpell
{
    private float _wetTime = 5f;
    private Projectile _projectile;
    private ParticleSystem _splashEffect;
    private float _knockbackForce;
    private float _launchSpeed;

    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        WaterBucketSpellConfig spellConfig = config as WaterBucketSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type WaterBucketSpellConfig");
        _wetTime = spellConfig.wetTime;
        _knockbackForce = spellConfig.knockbackForce;
        _launchSpeed = spellConfig.launchSpeed;
        _splashEffect = spellConfig.splashEffect;
        _projectile = spellConfig.projectile;
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return Drench.CanAffect(enemy);
    }
    public (Vector3 position, Quaternion rotation) GetLaunchPlatform()
    {
        Transform launch = ServiceLocator.Instance.Get<IGameManager>().GetGame().Player.GetLaunchPosition();
        return (launch.position, launch.rotation);
    }

    protected override void Apply(Enemy spellTarget)
    {
        Projectile projectile = Instantiate(_projectile);
        projectile.Init(this, spellTarget, _launchSpeed);
    }
    
    public void ApplyEnemy(Enemy enemy, Projectile projectile)
    {
        Drench drench = new(Status.Wet, _wetTime);
        enemy.ApplyStatus(drench);
        
        // Spawn the explosion effect
        ParticleSystem explosion = Instantiate(_splashEffect);
        explosion.transform.position = enemy.GetCenter();
        explosion.Play(true);

        // Destroy the projectile
        Destroy(projectile.gameObject);
    }
}