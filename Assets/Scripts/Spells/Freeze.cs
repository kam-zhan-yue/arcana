using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Freeze : Spell
{
    [SerializeField] private float freezeTime = 5f;
    
    protected override void Apply(ISpellTarget spellTarget)
    {
        if (spellTarget.GetTransform().TryGetComponent(out IFreezeTarget freezeTarget))
        {
            freezeTarget.Freeze(freezeTime);
        }
    }
    
    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        TypeSetting typeSetting = settings.GetSettingForType(DamageType.Ice);
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetPulse(typeSetting.colour, settings.GetPulseAmount(interactingTime));
        }
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisablePulse();
        }
    }
}
