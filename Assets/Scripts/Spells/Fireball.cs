using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField] private float launchSpeed = 20f;
    [SerializeField] private FireballProjectile fireballPrefab;
    
    protected override void Apply(ISpellTarget target)
    {
        Debug.Log("Fireballing " + target.GetTransform().gameObject.name);

        // Init the fireball
        Transform launchTransform = ServiceLocator.Instance.Get<IGameManager>().GetPlayer().GetLaunchPosition();
        FireballProjectile fireball = Instantiate(fireballPrefab);
        fireball.transform.SetPositionAndRotation(launchTransform.position, launchTransform.rotation);
        
        // Launch the fireball
        Vector3 targetPosition = target.GetTransform().position;
        Vector3 launchDirection = targetPosition - fireball.transform.position;
        Debug.Log("Launch Direction is " + launchDirection);
        Vector3 launchForce = launchDirection * launchSpeed;
        fireball.Rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }
}
