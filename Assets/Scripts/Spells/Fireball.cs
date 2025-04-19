using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Fireball : Spell
{
    private float _burnTime;
    private float _burnDamage;
    private float _burnTick;
    private float _launchSpeed;
    private float _knockbackForce;
    private float _knockbackTime; 
    private FireballProjectile _fireballPrefab;

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
        _fireballPrefab = fireballConfig.fireballPrefab;
    }

    protected override List<Enemy> GetTargets()
    {
        Enemy currentTarget = GetCurrentTarget();
        if (currentTarget && Burn.CanAffect(currentTarget))
            return new List<Enemy> { currentTarget };
        return new();
    }

    protected override void Apply(Enemy spellTarget)
    {
        Debug.Log($"Applying Fireball to {spellTarget.name}");
        
        // Init the fireball
        Transform launchPosition = ServiceLocator.Instance.Get<IGameManager>().GetPlayer().GetLaunchPosition();
        FireballProjectile fireball = Instantiate(_fireballPrefab);
        fireball.Init(this);
        fireball.transform.SetPositionAndRotation(launchPosition.position, launchPosition.rotation);
        
        // Launch the fireball
        Vector3 targetPosition = spellTarget.GetTransform().position;
        Vector3 launchDirection = targetPosition - fireball.transform.position;
        Debug.Log("Launch Direction is " + launchDirection);
        Vector3 launchForce = launchDirection * _launchSpeed;
        fireball.Rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }
    
    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Fire);
        
        for (int i = 0; i < enemies.Count; ++i)
        {
            if(Burn.CanAffect(enemies[i]))
                enemies[i].SetOutline(typeSetting.colour, settings.outlineSize);
        }

        Enemy targetedEnemy = GetCurrentTarget();
        if (targetedEnemy && Burn.CanAffect(targetedEnemy))
        {
            targetedEnemy.SetOutline(settings.selectColour, settings.outlineSize);
        }
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisableOutline();
        }
    }

    public void ApplyEnemy(Enemy enemy, FireballProjectile projectile)
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
