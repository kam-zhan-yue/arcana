using Kuroneko.UIDelivery;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : Popup
{
    [SerializeField] private Button restartButton;
    
    protected override void InitPopup()
    {
        restartButton.onClick.AddListener(RestartButtonClicked);
    }

    private void RestartButtonClicked()
    {
        
    }
}
