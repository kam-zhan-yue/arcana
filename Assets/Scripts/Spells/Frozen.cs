using UnityEngine;

public class Frozen : StatusEffect
{
    public Frozen(Status s, float t) : base(s, t)
    {
    }

    public static bool CanAffect(Enemy enemy)
    {
        return true;
    }


    public override StatusEffect Clone()
    {
        return new Frozen(status, statusTime);
    }

    protected override bool CanApply(Enemy enemy)
    {
        return CanAffect(enemy);
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