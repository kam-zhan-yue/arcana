using System;

public class Thunder : SingleTargetSpell
{
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        ThunderSpellConfig spellConfig = config as ThunderSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type ThunderSpellConfig.");
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return true;
    }

    protected override void Apply(Enemy spellTarget)
    {
        Damage spellDamage = new (damage, DamageType.Fire, DamageEffect.None);
        spellTarget.Damage(spellDamage);
    }
}
