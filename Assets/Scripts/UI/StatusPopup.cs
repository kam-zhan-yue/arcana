using Cysharp.Threading.Tasks;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;

public class StatusPopup : Popup
{
    [SerializeField] private TMP_Text statusText;
    
    protected override void InitPopup()
    {
        HidePopup();
    }
    
    public void Init(Enemy enemy)
    {
        enemy.OnStatus += OnStatus;
    }

    private void OnStatus(Status status)
    {
        if (status == Status.None)
            HidePopup();
        else
        {
            ShowPopup();
            statusText.text = status.ToString();
        }
    }
}
