using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class WaterBucket : Spell
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
    protected override List<Enemy> GetTargets()
    {
        return !cardPopup.CanActivate ? new List<Enemy>() : GetFilteredTargets();
    }

    private List<Enemy> GetFilteredTargets()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        List<Enemy> targets = new();
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Frozen.CanAffect(enemies[i]))
                targets.Add(enemies[i]);
        }
        return targets;
    }

    protected override void Apply(Enemy spellTarget)
    {
        Drench drench = new(Status.Wet, _wetTime);
        spellTarget.ApplyStatus(drench);
    }

    protected override void OnStartDragging()
    {
        base.OnStartDragging();
        cardPopup.EnableActivationZone();
    }

    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = GetFilteredTargets();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Water);
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetOutline(cardPopup.CanActivate ? settings.selectColour : typeSetting.colour, settings.outlineSize);
        }
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        cardPopup.DisableActivationZone();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisableOutline();
        }
    }
}