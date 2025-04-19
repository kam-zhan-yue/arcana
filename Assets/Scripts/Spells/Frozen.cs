public class Frozen : StatusEffect
{
    public Frozen(Status s, float t) : base(s, t)
    {
    }

    protected override bool CanApply(Enemy enemy)
    {
        return enemy.Status != Status.Burned;
    }

    protected override void OnApply(Enemy enemy)
    {
        enemy.Rigidbody.isKinematic = true;
    }

    protected override void OnUpdate(Enemy enemy, float deltaTime)
    {
    }

    protected override void OnComplete(Enemy enemy)
    {
        enemy.Rigidbody.isKinematic = false;
    }
}