using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    [SerializeField] private GameDatabase gameDatabase;

    [NonSerialized, ShowInInspector, ReadOnly]
    private Game _game;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
        Player player = FindAnyObjectByType<Player>();
        _game = new Game(player, gameDatabase);
    }
    
    public GameDatabase GetGameDatabase()
    {
        return gameDatabase;
    }
    
    public Game GetGame()
    {
        return _game;
    }
}
