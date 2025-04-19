using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Freeze : Spell
{
    [SerializeField] private float freezeTime = 5f;
    
    protected override void Apply(ISpellTarget target)
    {
        if (target.GetTransform().TryGetComponent(out IFreezeTarget freezeTarget))
        {
            freezeTarget.Freeze(freezeTime);
        }
    }

    protected override void Hover()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Ice);
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetOutline(typeSetting.colour, settings.outlineSize);
        }
    }

    protected override void UnHover()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisableOutline();
        }
    }
}
