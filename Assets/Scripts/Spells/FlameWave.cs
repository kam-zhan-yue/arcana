using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public class FlameWave : Spell
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
            if (Burn.CanAffect(enemies[i]))
                targets.Add(enemies[i]);
        }
        return targets;
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
    
    protected override void OnStartDragging()
    {
        base.OnStartDragging();
        cardPopup.EnableActivationZone();
    }

    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = GetFilteredTargets();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Fire);
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