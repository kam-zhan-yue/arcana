using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public class Thunder : Spell
{
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        ThunderSpellConfig spellConfig = config as ThunderSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type ThunderSpellConfig.");
    }
    
    protected override List<Enemy> GetTargets()
    {
        Enemy currentTarget = GetCurrentTarget();
        if (currentTarget)
            return new List<Enemy> { currentTarget };
        return new();
    }

    protected override void Apply(Enemy spellTarget)
    {
        Damage spellDamage = new (damage, DamageType.Fire, DamageEffect.None);
        spellTarget.Damage(spellDamage);
    }
    
    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Electric);
        
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetOutline(typeSetting.colour, settings.outlineSize);
        }

        Enemy targetedEnemy = GetCurrentTarget();
        if (targetedEnemy && Burn.CanAffect(targetedEnemy))
        {
            targetedEnemy.SetOutline(settings.selectColour, settings.outlineSize);
        }
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisableOutline();
        }
    }
}
