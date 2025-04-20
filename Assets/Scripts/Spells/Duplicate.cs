using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Duplicate : ActivationSpell
{
    protected override List<Enemy> GetTargets()
    {
        return new();
    }

    protected override bool CanApply()
    {
        List<CardType> cardHistory = ServiceLocator.Instance.Get<IGameManager>().GetCardHistory();
        return base.CanApply() && cardHistory.Count > 0;
    }

    protected override void Apply(Enemy spellTarget)
    {
    }

    protected override void Use()
    {
        List<CardType> cardHistory = ServiceLocator.Instance.Get<IGameManager>().GetCardHistory();
        // Adds the last played card twice
        ServiceLocator.Instance.Get<IGameManager>().AddCard(cardHistory[^1]);
        ServiceLocator.Instance.Get<IGameManager>().AddCard(cardHistory[^1]);
        base.Use();
    }
}
