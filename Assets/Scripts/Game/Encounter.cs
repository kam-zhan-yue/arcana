using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Encounter
{
    [InlineProperty, SerializeField] private CardType[] cards = Array.Empty<CardType>();
    [InlineProperty, SerializeField] private List<EncounterStep> steps = new();
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    public async UniTask Play(CancellationToken token)
    {
        await AddCardsAsync(token);
        for (int i = 0; i < steps.Count; ++i)
        {
            await steps[i].Play(this, token);
        }
    }

    private async UniTask AddCardsAsync(CancellationToken token)
    {
        Game game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        for (int i = 0; i < cards.Length; ++i)
        {
            game.AddCard(cards[i]);
            await UniTask.WaitForSeconds(0.2f, cancellationToken: token);
        }
    }
    
    public void Resolve()
    {
        
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        ServiceLocator.Instance.Get<IGameManager>().GetGame().AddActiveEnemy(enemy);
        enemy.OnRelease += RemoveEnemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        ServiceLocator.Instance.Get<IGameManager>().GetGame().RemoveActiveEnemy(enemy);
        enemy.OnRelease -= RemoveEnemy;
    }
    
    
    [HorizontalGroup()]
    [Button((ButtonSizes.Medium)), GUIColor(0.2f, 1f, 0)]
    public void AddStep()
    {
        steps.Add(new EncounterStep());
    }
}
