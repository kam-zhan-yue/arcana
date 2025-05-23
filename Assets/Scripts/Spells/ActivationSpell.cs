using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public abstract class ActivationSpell : Spell
{
    protected override bool CanApply()
    {
        return cardPopup.CanActivate;
    }
    
    protected override void OnStartDragging()
    {
        base.OnStartDragging();
        cardPopup.CardPanelPopup.EnableActivationZone();
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        cardPopup.CardPanelPopup.DisableActivationZone();
    }
}