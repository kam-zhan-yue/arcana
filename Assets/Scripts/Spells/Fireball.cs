using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField] private float launchSpeed = 20f;
    [SerializeField] private float knockbackForce = 20f;
    [SerializeField] private float knockbackTime = 0.2f;
    [SerializeField] private FireballProjectile fireballPrefab;
    
    protected override void Apply(ISpellTarget target)
    {
        Debug.Log("Fireballing " + target.GetTransform().gameObject.name);

        // Init the fireball
        Transform launchPosition = ServiceLocator.Instance.Get<IGameManager>().GetPlayer().GetLaunchPosition();
        FireballProjectile fireball = Instantiate(fireballPrefab);
        fireball.Init(this);
        fireball.transform.SetPositionAndRotation(launchPosition.position, launchPosition.rotation);
        
        // Launch the fireball
        Vector3 targetPosition = target.GetTransform().position;
        Vector3 launchDirection = targetPosition - fireball.transform.position;
        Debug.Log("Launch Direction is " + launchDirection);
        Vector3 launchForce = launchDirection * launchSpeed;
        fireball.Rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }

    protected override void Hover()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            Debug.Log($"Highlight {enemies[i].name}");
        }
    }

    protected override void UnHover()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            Debug.Log($"Unhighlight {enemies[i].name}");
        }
    }

    public void ApplyEnemy(Enemy enemy, FireballProjectile projectile)
    {
        float multiplier = 1f;
        
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Frozen)
        {
            effect = DamageEffect.Melt;
            enemy.Unfreeze();
        }

        Damage fireballDamage = new Damage(damage, DamageType.Fire, effect);
        enemy.Damage(fireballDamage);

        // Knockback the enemy only if they die from the fireball
        if (enemy.IsDead)
        {
            Vector3 direction = projectile.Rigidbody.linearVelocity;
            enemy.Knockback(direction.normalized * knockbackForce * multiplier, knockbackTime);
        }
        
        // Destroy the fireball projectile
        Destroy(projectile.gameObject);
    }
}
