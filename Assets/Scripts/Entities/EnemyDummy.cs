using Kuroneko.UtilityDelivery;

public class EnemyDummy : Enemy
{
    protected override void OnInit(EnemyData data)
    {
    }

    protected override void Move()
    {
    }

    protected override void Attack()
    {
        Player player = ServiceLocator.Instance.Get<IGameManager>().GetPlayer();
        player.Damage();
    }
}
