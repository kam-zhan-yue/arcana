using System;
using UnityEngine;

public class Freeze : MultiTargetSpell
{
    private float _freezeTime = 5f;

    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        FreezeSpellConfig freezeConfig = config as FreezeSpellConfig;
        if (freezeConfig == null)
            throw new InvalidCastException("Config must be of type FreezeSpellConfig.");
        _freezeTime = freezeConfig.freezeTime;
    }
    
    protected override bool CanAffect(Enemy enemy)
    {
        return Frozen.CanAffect(enemy);
    }
    
    protected override void Apply(Enemy spellTarget)
    {
        Debug.Log($"Applying Freeze to {spellTarget.name}");
        Frozen frozen = new(Status.Frozen, _freezeTime);
        spellTarget.ApplyStatus(frozen);
        // Deal 0 damage to apply the pulse effect
        Damage spellDamage = new Damage(0f, DamageType.Water, DamageEffect.None);
        spellTarget.Damage(spellDamage);
    }

    protected override void Use()
    {
        base.Use();
        AudioManager.instance.Play("SFX_FREEZE_CAST");
    }
}
