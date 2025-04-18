using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EnemyBasic : Enemy
{
    public override void Move()
    {
        Player player = ServiceLocator.Instance.Get<IGameManager>().GetPlayer();
        Vector3 nextPosition =
            Vector3.MoveTowards(rb.position, player.transform.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(nextPosition);
    }
}
