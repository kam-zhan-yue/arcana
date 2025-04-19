public class Drench : StatusEffect
{
    public Drench(Status s, float t) : base(s, t)
    {
    }

    public static bool CanAffect(Enemy enemy)
    {
        return enemy.Status != Status.Frozen;
    }

    protected override bool CanApply(Enemy enemy)
    {
        return CanAffect(enemy);
    }

    protected override void OnApply(Enemy enemy)
    {
        // If burned, just put it out
        if (enemy.Status == Status.Burned)
        {
            status = Status.None;
            completed = true;
        }
    }

    protected override void OnUpdate(Enemy enemy, float deltaTime)
    {
    }

    protected override void OnComplete(Enemy enemy)
    {
    }
}