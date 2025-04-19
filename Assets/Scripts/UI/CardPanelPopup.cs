using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CardPanelPopup : Popup
{
    [SerializeField] private ActivationZone activationZone;
    public bool CanActivate => activationZone.CanActivate;
    
    protected override void InitPopup()
    {
        DisableActivationZone();
    }
    
    public void EnableActivationZone()
    {
        ShowPopup();
    }

    public void DisableActivationZone()
    {
        HidePopup();
    }
}
