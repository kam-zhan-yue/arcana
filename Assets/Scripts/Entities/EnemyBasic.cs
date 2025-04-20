using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EnemyBasic : Enemy
{
    protected override void OnInit(EnemyData data)
    {
        
    }

    protected override void Move()
    {
        Player player = GetPlayer();
        Vector3 direction = (player.transform.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation( direction.normalized, Vector3.up);
            rb.MoveRotation(lookRotation);
        }
        Debug.Log($"Velocity is {Rigidbody.linearVelocity}");
    }

    protected override void Attack()
    {
        animator.Play("MeleeAttack");
        Player player = GetPlayer();
        player.Damage();
    }
}
