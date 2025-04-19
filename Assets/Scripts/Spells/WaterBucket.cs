using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class WaterBucket : Spell
{
    [SerializeField] private float wetTime = 5f;

    protected override List<Enemy> GetTargets()
    {
        if (!cardPopup.CanActivate)
            return new();
        
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        List<Enemy> targets = new();
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Drench.CanAffect(enemies[i]))
                targets.Add(enemies[i]);
        }
        return targets;
    }

    protected override void Apply(Enemy spellTarget)
    {
        Drench drench = new(Status.Wet, wetTime);
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
        List<Enemy> enemies = GetTargets();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Ice);
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