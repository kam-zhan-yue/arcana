using System;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class MainMenuPopup : Popup
{
    [SerializeField] private TextPopupItem playButton;
    
    private Game _game;
    protected override void InitPopup()
    {
        HidePopup();
        _game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        _game.OnMainMenu += ShowPopup;
        playButton.OnClick += OnPlayClicked;
    }

    private void OnPlayClicked()
    {
        ServiceLocator.Instance.Get<IGameManager>().StartGame();
    }
    
    private void OnDestroy()
    {
        _game.OnMainMenu -= ShowPopup;
        playButton.OnClick -= OnPlayClicked;
    }
}
