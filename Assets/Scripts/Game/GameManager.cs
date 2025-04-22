using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private GameDatabase gameDatabase;

    [NonSerialized, ShowInInspector, ReadOnly]
    private Game _game;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
        Player player = FindAnyObjectByType<Player>();
        GameFlow flow = FindAnyObjectByType<GameFlow>();
        _game = new Game(player, flow, gameDatabase);
    }

    private void Start()
    {
        if (_game.Flow.StartStep == 0 && _game.Database.settings.levels[0].buildIndex == SceneManager.GetActiveScene().buildIndex)
            _game.ShowMainMenu();
        else
            StartGame();
    }
    
    public Game GetGame()
    {
        return _game;
    }

    public void StartGame()
    {
        _game.Start();
    }
}
