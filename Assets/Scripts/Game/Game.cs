using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Game : MonoBehaviour, IGameManager
{
    [SerializeField] private Player player;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
    }
    
    public Player GetPlayer()
    {
        return player;
    }
}
