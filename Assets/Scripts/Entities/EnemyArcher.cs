using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EnemyArcher : Enemy
{
    [SerializeField] private Crossbow crossbow;
    
    private static readonly int RangedAttack = Animator.StringToHash("RangedAttack");
    private static readonly int AimStart = Animator.StringToHash("AimStart");

    protected override void Awake()
    {
        base.Awake();
        crossbow.gameObject.SetActiveFast(false);
    }

    protected override void OnInit(EnemyData data)
    {
        base.OnInit(data);
        crossbow.gameObject.SetActiveFast(true);
        animator.SetTrigger(AimStart);
    }

    protected override void Move()
    {
    }

    protected override void Attack()
    {
        Debug.Log("Enemy Archer Attack");
        animator.SetTrigger(RangedAttack);
        Player player = GetPlayer();
        player.Damage();
    }
}
