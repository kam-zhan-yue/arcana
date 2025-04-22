using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CardPanelPopup : Popup
{
    [SerializeField] private ActivationZone activationZone;
    public bool CanActivate => isShowing && activationZone.CanActivate;
    public ActivationZone ActivationZone => activationZone;
    
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
