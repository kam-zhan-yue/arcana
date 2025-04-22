using System;
using UnityEngine;

public class Duck : SingleTargetSpell
{
    private GameObject _duckPrefab;
    
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        DuckSpellConfig spellConfig = config as DuckSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type DuckSpellConfig");
        _duckPrefab = spellConfig.duckPrefab;
    }

    protected override void Use()
    {
        base.Use();
        AudioManager.instance.Play("SFX_DUCK");
    }

    protected override void Apply(Enemy spellTarget)
    {
        spellTarget.Mutate(_duckPrefab);
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return true;
    }
}
