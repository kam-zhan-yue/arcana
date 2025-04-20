using Kuroneko.UtilityDelivery;
using UnityEngine;

public class EnemyMage : Enemy
{
    protected override void OnInit()
    {
    }

    protected override void Move()
    {
        Player player = ServiceLocator.Instance.Get<IGameManager>().GetPlayer();
        Vector3 direction = player.transform.position - rb.position;

        Vector3 nextPosition = Vector3.MoveTowards(rb.position, player.transform.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(nextPosition);

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            rb.MoveRotation(lookRotation);
        }
    }
}
