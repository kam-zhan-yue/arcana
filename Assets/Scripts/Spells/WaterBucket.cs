using System;

public class WaterBucket : MultiTargetSpell
{
    private float _wetTime = 5f;

    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        WaterBucketSpellConfig spellConfig = config as WaterBucketSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type WaterBucketSpellConfig");
        _wetTime = spellConfig.wetTime;
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return Drench.CanAffect(enemy);
    }

    protected override void Apply(Enemy spellTarget)
    {
        Drench drench = new(Status.Wet, _wetTime);
        spellTarget.ApplyStatus(drench);
    }
}