public class Burn : StatusEffect
{
    private float _burnDamage;
    private float _burnTick;
    private float _burnTimer;
    
    public Burn(Status s, float t, float burnDamage, float burnTick) : base(s, t)
    {
        _burnDamage = burnDamage;
        _burnTick = burnTick;
    }

    public static bool CanAffect(Enemy enemy)
    {
        return enemy.Status != Status.Wet;
    }

    public override StatusEffect Clone()
    {
        return new Burn(status, statusTime, _burnDamage, _burnTick);
    }

    protected override bool CanApply(Enemy enemy)
    {
        return CanAffect(enemy);
    }

    public override void Refresh()
    {
        _burnTimer = 0f;
    }

    protected override void OnApply(Enemy enemy)
    {
        // If the enemy is frozen, we want to cancel it out
        if (enemy.Status == Status.Frozen)
        {
            status = Status.None;
            completed = true;
        }
        else
        {
            _burnTimer = 0f;
        }
    }

    protected override void OnUpdate(Enemy enemy, float deltaTime)
    {
        _burnTimer += deltaTime;
        if (_burnTimer >= _burnTick)
        {
            _burnTimer -= _burnTick;
            ApplyBurn(enemy);
        }
    }

    protected override void OnComplete(Enemy enemy)
    {
    }

    private void ApplyBurn(Enemy enemy)
    {
        Damage burnDamage = new(_burnDamage, DamageType.Fire, DamageEffect.None);
        enemy.Damage(burnDamage);
    }
}