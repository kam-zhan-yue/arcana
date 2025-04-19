using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Freeze : Spell
{
    [SerializeField] private float freezeTime = 5f;

    protected override List<Enemy> GetTargets()
    {
        if (cardPopup.CanActivate)
        {
            List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
            return enemies;
        }
        else
        {
            return new();
        }
    }

    protected override void Apply(Enemy spellTarget)
    {
        Debug.Log($"Applying Freeze to {spellTarget.name}");
        spellTarget.Freeze(freezeTime);
    }

    protected override void OnStartDragging()
    {
        base.OnStartDragging();
        cardPopup.EnableActivationZone();
    }

    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
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
