using System;

public class Lightning : MultiTargetSpell
{
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        LightningSpellConfig spellConfig = config as LightningSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type LightningSpellConfig.");
    }

    protected override void Apply(Enemy enemy)
    {
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Wet)
        {
            effect = DamageEffect.Electrocute;
        }
        Damage spellDamage = new (damage, DamageType.Electric, effect);
        enemy.Damage(spellDamage);
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return enemy.Status == Status.Wet;
    }
}