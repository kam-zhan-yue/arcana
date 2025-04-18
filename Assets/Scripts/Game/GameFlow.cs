using System;
using System.Collections.Generic;
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
    [SerializeField] private List<GameStep> steps = new();

    private void Awake()
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

    private void Start()
    {
        PlayFlow().Forget();
    }

    private async UniTask PlayFlow()
    {
        for (int i = startStep; i < steps.Count; ++i)
        {
            await steps[i].Play();
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
