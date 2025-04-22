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
    public CardPanelPopup CardPanelPopup => cardPanelPopup;
    
    protected override void InitPopup()
    {
        cardPanelPopup.DisableActivationZone();
        cardContainer.Init(this);
    }

    public void RemoveCard(CardPopupItem cardPopupItem)
    {
        cardContainer.RemoveCard(cardPopupItem);
    }
}
