using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class PausePopup : Popup
{
    private Game _game;
    
    protected override void InitPopup()
    {
        HidePopup();
    }
    
    private void Start()
    {
        _game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isShowing && _game.IsPaused)
        {
            HidePopup();
            _game.Unpause();
        }
        else if (_game.CanPause && !_game.IsPaused)
        {
            ShowPopup();
            _game.Pause();
        }
    }
}
