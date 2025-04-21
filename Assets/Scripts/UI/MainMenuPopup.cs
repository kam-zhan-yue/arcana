using System;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class MainMenuPopup : Popup
{
    private Game _game;
    private bool _started = false;
    protected override void InitPopup()
    {
        HidePopup();
        _game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        _game.OnMainMenu += ShowPopup;
    }

    private void Update()
    {
        if (isShowing && !_started && Input.GetMouseButtonDown(0))
        {
            _started = true;
            ServiceLocator.Instance.Get<IGameManager>().StartGame();
        }
    }
    
    private void OnDestroy()
    {
        _game.OnMainMenu -= ShowPopup;
    }
}
