using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    [NonSerialized, ShowInInspector, ReadOnly]
    private Player _player;

    [NonSerialized, ShowInInspector, ReadOnly]
    private List<Enemy> _enemies = new();
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
        _player = FindAnyObjectByType<Player>();
    }
    
    public Player GetPlayer()
    {
        return _player;
    }

    public List<Enemy> GetActiveEnemies()
    {
        return _enemies;
    }

    public void AddActiveEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveActiveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
}
