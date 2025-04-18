using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Encounter
{
    [SerializeField] private List<EncounterStep> steps = new();
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    public async UniTask Play()
    {
        for (int i = 0; i < steps.Count; ++i)
        {
            await steps[i].Play(this);
        }
    }
    
    public void Resolve()
    {
        
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.OnRelease += () => enemies.Remove(enemy);
    }
    
    
    [HorizontalGroup()]
    [Button((ButtonSizes.Medium)), GUIColor(0.2f, 1f, 0)]
    public void AddStep()
    {
        steps.Add(new EncounterStep());
    }
}
