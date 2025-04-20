using UnityEngine;

public class EnemyBasic : Enemy
{
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    protected override void OnInit(EnemyData data)
    {
        
    }

    protected override void Move()
    {
        Player player = GetPlayer();
        Vector3 direction = (player.transform.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    protected override void Attack()
    {
        animator.SetTrigger(AttackTrigger);
        Player player = GetPlayer();
        player.Damage();
    }
}
