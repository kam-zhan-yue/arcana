using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Heal : ActivationSpell
{
    protected override List<Enemy> GetTargets()
    {
        return new();
    }

    protected override void Apply(Enemy spellTarget)
    {
    }

    protected override void Use()
    {
        ServiceLocator.Instance.Get<IGameManager>().GetPlayer().Heal();
        base.Use();
    }
}