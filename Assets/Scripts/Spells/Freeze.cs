using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Freeze : Spell
{
    [SerializeField] private float freezeTime = 5f;
    
    protected override void Apply(Enemy spellTarget)
    {
        spellTarget.Freeze(freezeTime);
    }
    
    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Ice);
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetOutline(typeSetting.colour, settings.outlineSize);
        }

        Enemy targetedEnemy = GetCurrentTarget();
        if (targetedEnemy)
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
