using System;
using UnityEngine;


public class GameFlow : MonoBehaviour
{
    // Hold all of the player animations
    // When the player animation stops, it needs to trigger an enemy encounter, or similar. An encounter.
    // After the encounter, it will play the next one.
    // It will linearly be outlined here. Easy Peasy Lemon Squeezy.

    [SerializeField] private int startStep;
    [SerializeField] private GameStep[] steps = Array.Empty<GameStep>();

    private void Awake()
    {
        if (startStep > steps.Length)
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
        PlayStep(startStep);
    }

    private void PlayStep(int index)
    {
        steps[index].Play();
    }
}
