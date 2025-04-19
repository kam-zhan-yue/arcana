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
        OnStatusAsync(status).Forget();
    }

    private async UniTask OnStatusAsync(Status status)
    {
        ShowPopup();
        statusText.text = status.ToString();
        await UniTask.WaitForSeconds(2f);
        HidePopup();
    }
}
