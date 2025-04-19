using System;

public class FlameWave : MultiTargetSpell
{
    private float _burnTime;
    private float _burnDamage;
    private float _burnTick;
    
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        FlameWaveSpellConfig spellConfig = config as FlameWaveSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type FlameWaveSpellConfig.");
        _burnTime = spellConfig.burnTime;
        _burnDamage = spellConfig.burnDamage;
        _burnTick = spellConfig.burnTick;
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return Burn.CanAffect(enemy);
    }

    protected override void Apply(Enemy enemy)
    {
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Frozen)
        {
            effect = DamageEffect.Melt;
        }
        Burn burn = new (Status.Burned, _burnTime, _burnDamage, _burnTick);
        Damage spellDamage = new (damage, DamageType.Fire, effect);
        enemy.ApplyStatus(burn);
        enemy.Damage(spellDamage);
    }
}