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
    }
}
