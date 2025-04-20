using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private IProjectileSpell _spell;
    public Vector3 GetDirection => _rigidbody.linearVelocity.normalized;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(IProjectileSpell spell, Enemy enemy, float launchSpeed)
    {
        _spell = spell;

        (Vector3 position, Quaternion rotation) launch = spell.GetLaunchPlatform();
        transform.SetPositionAndRotation(launch.position, launch.rotation);
        
        // Launch the fireball
        Vector3 targetPosition = enemy.GetCenter();
        Vector3 launchDirection = targetPosition - transform.position;
        Vector3 launchForce = launchDirection * launchSpeed;
        _rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            _spell.ApplyEnemy(enemy, this);
        }
    }
}