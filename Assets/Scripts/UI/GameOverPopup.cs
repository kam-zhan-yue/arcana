using System;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : Popup
{
    private Game _game;
    
    protected override void InitPopup()
    {
        HidePopup();
    }

    private void Start()
    {
        _game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        _game.OnEndGame += OnEndGame;
    }

    private void OnEndGame()
    {
        ShowPopup();
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isShowing && Input.GetMouseButtonDown(0))
        {
            RestartButtonClicked();
        }
    }

    private void RestartButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        _game.OnEndGame -= OnEndGame;
    }
}
