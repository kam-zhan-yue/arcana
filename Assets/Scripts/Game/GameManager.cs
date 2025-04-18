using System;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    [NonSerialized, ShowInInspector, ReadOnly]
    private Player _player;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
        _player = FindAnyObjectByType<Player>();
    }
    
    public Player GetPlayer()
    {
        return _player;
    }
}
