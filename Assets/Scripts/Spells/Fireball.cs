using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField] private float launchSpeed = 20f;
    [SerializeField] private float knockbackForce = 20f;
    [SerializeField] private float knockbackTime = 0.2f;
    [SerializeField] private float meltMultiplier = 2f;
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

    public void ApplyEnemy(Enemy enemy, FireballProjectile projectile)
    {
        float multiplier = 1f;
        if (enemy.Status == Status.Frozen)
        {
            multiplier = meltMultiplier;
            enemy.Unfreeze();
        }
        Vector3 direction = projectile.Rigidbody.linearVelocity;
        enemy.Knockback(direction.normalized * knockbackForce * multiplier, knockbackTime);
    }
}
