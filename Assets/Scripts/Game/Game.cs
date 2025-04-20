using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

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

    public Game(Player player, GameDatabase database)
    {
        _player = player;
        _database = database;
        _player.Init(this);
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
        OnEndGame?.Invoke();
    }
}