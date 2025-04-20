using System;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private IProjectileEnemy _enemy;
    public Vector3 GetDirection => _rigidbody.linearVelocity.normalized;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(IProjectileEnemy enemy, Player player, Transform launchTransform, float launchSpeed)
    {
        _enemy = enemy;

        transform.SetPositionAndRotation(launchTransform.position, launchTransform.rotation);

        Vector3 targetPosition = player.GetHead().position;
        Vector3 launchDirection = targetPosition - transform.position;
        Vector3 launchForce = launchDirection * launchSpeed;
        _rigidbody.AddForce(launchForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _enemy.ApplyPlayer(player, this);
        }
    }
}