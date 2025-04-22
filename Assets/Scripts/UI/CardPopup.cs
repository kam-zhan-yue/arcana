using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CardPopup : Popup
{
    [SerializeField] private TooltipPopup tooltipPopup;
    [SerializeField] private CardPanelPopup cardPanelPopup;
    [SerializeField] private CardContainer cardContainer;
    [SerializeField] private VisualCardContainer visualCardContainer;

    public bool CanActivate => cardPanelPopup.CanActivate;
    public TooltipPopup TooltipPopup => tooltipPopup;
    
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

    public void RemoveCard(CardPopupItem cardPopupItem)
    {
        cardContainer.RemoveCard(cardPopupItem);
    }
}
