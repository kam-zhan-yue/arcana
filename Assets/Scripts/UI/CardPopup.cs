using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CardPopup : Popup
{
    [SerializeField] private CardPanelPopup cardPanelPopup;
    [SerializeField] private CardContainer cardContainer;
    [SerializeField] private VisualCardContainer visualCardContainer;

    public bool CanActivate => cardPanelPopup.CanActivate;
    
    protected override void InitPopup()
    {
        DisableActivationZone();
        cardContainer.Init(this);
    }

    public void EnableActivationZone()
    {
        cardPanelPopup.EnableActivationZone();
    }

    public void DisableActivationZone()
    {
        cardPanelPopup.DisableActivationZone();
    }
}
