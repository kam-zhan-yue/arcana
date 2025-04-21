using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    Over,
}

[Serializable]
public class Game
{
    
    [NonSerialized, ShowInInspector, ReadOnly]
    private Player _player;

    [NonSerialized, ShowInInspector, ReadOnly]
    private List<Enemy> _enemies = new();

    [NonSerialized, ShowInInspector, ReadOnly]
    private List<CardType> _cardHistory = new();

    public Player Player => _player;
    public List<Enemy> Enemies => _enemies;
    public List<CardType> CardHistory => _cardHistory;
    private GameDatabase _database;
    public GameDatabase Database => _database;
    public Action<CardType> OnCardAdded;
    public Action OnEndGame;

    private GameState _state = GameState.Playing;

    public bool CanPause => _state == GameState.Playing;
    public bool IsPaused => _state == GameState.Paused;
    private GameFlow _flow;

    public Game(Player player, GameFlow flow, GameDatabase database)
    {
        _player = player;
        _flow = flow;
        _database = database;
        _player.Init(this);
    }

    public void Start()
    {
        _flow.PlayFlow();
    }

    public void UseCard(CardType cardType)
    {
        _cardHistory.Add(cardType);
    }

    public void AddCard(CardType cardType)
    {
        OnCardAdded?.Invoke(cardType);
    }
    
    public void AddActiveEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveActiveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    public void EndGame()
    {
        _state = GameState.Over;
        OnEndGame?.Invoke();
    }

    public void Pause()
    {
        _state = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        _state = GameState.Playing;
        Time.timeScale = 1f;
    }
}