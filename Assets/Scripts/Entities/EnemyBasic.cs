using UnityEngine;

public class EnemyBasic : Enemy
{
    private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");

    protected override void PlayAttackAnimation()
    {
        animator.SetTrigger(MeleeAttack);
    }

    protected override void Move()
    {
        Player player = GetPlayer();
        Vector3 direction = (player.transform.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    protected override void Attack()
    {
        Player player = GetPlayer();
        player.Damage();
    }
}
