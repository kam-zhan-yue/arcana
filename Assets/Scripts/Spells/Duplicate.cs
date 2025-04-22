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
        List<CardType> cardHistory = ServiceLocator.Instance.Get<IGameManager>().GetGame().CardHistory;
        return base.CanApply() && cardHistory.Count > 0;
    }

    protected override void Apply(Enemy spellTarget)
    {
    }

    protected override void Use()
    {
        Game game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        List<CardType> cardHistory = game.CardHistory;
        // Adds the last played card twice
        game.AddCard(cardHistory[^1]);
        game.AddCard(cardHistory[^1]);
        base.Use();
        AudioManager.instance.Play("SFX_DUPLICATE");
    }
}
