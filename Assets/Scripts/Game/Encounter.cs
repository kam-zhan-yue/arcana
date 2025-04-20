using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Encounter
{
    [SerializeField] private CardType[] cards = Array.Empty<CardType>();
    [SerializeField] private List<EncounterStep> steps = new();
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    public async UniTask Play()
    {
        for (int i = 0; i < cards.Length; ++i)
        {
            ServiceLocator.Instance.Get<IGameManager>().AddCard(cards[i]);
        }
        for (int i = 0; i < steps.Count; ++i)
        {
            await steps[i].Play(this);
        }
        // ServiceLocator.Instance.Get<IGameManager>().ClearHand();
    }
    
    public void Resolve()
    {
        
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        ServiceLocator.Instance.Get<IGameManager>().AddActiveEnemy(enemy);
        enemy.OnRelease += RemoveEnemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        ServiceLocator.Instance.Get<IGameManager>().RemoveActiveEnemy(enemy);
        enemy.OnRelease -= RemoveEnemy;
    }
    
    
    [HorizontalGroup()]
    [Button((ButtonSizes.Medium)), GUIColor(0.2f, 1f, 0)]
    public void AddStep()
    {
        steps.Add(new EncounterStep());
    }
}
