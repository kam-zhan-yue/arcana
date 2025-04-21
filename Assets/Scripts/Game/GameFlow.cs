using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;


public class GameFlow : MonoBehaviour
{
    // Hold all of the player animations
    // When the player animation stops, it needs to trigger an enemy encounter, or similar. An encounter.
    // After the encounter, it will play the next one.
    // It will linearly be outlined here. Easy Peasy Lemon Squeezy.

    [SerializeField] private int startStep;
    [NonSerialized, ShowInInspector, ReadOnly]
    private int _currentStep;
    
    [SerializeField] private List<GameStep> steps = new();

    public int StartStep => startStep;

    public void PlayFlow()
    {
        Resolve();
        PlayFlowAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private void Resolve()
    {
        if (startStep > steps.Count)
        {
            Debug.LogError("Invalid Start Step defined in Game Flow.");
            enabled = false;
            return;
        }
        for (int i = 0; i < startStep; ++i)
        {
            steps[i].Resolve();
        }
    }

    private async UniTask PlayFlowAsync(CancellationToken token)
    {
        for (int i = startStep; i < steps.Count; ++i)
        {
            _currentStep = i;
            await steps[i].Play(token);
            Debug.Log("Next Step!");
        }
    }
    
    [HorizontalGroup()]
    [Button((ButtonSizes.Large)), GUIColor(0.2f, 1f, 0)]
    public void AddStep()
    {
        steps.Add(new GameStep());
    }
}
