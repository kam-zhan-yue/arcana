using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public class Lightning : Spell
{
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        LightningSpellConfig spellConfig = config as LightningSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type LightningSpellConfig.");
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
            targets.Add(enemies[i]);
        }
        return targets;
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
    
    protected override void OnStartDragging()
    {
        base.OnStartDragging();
        cardPopup.EnableActivationZone();
    }

    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = GetFilteredTargets();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Electric);
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