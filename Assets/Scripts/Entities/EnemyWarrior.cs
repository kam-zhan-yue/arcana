using UnityEngine;

public class EnemyWarrior : Enemy
{
    private static readonly int MeleeWeaponAttack = Animator.StringToHash("MeleeWeaponAttack");

    protected override void PlayAttackAnimation()
    {
        animator.SetTrigger(MeleeWeaponAttack);
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