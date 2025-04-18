using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public enum GameStepType
{
    Level,
    Encounter,
}

[Serializable]
public class GameStep
{
    public GameStepType type;
    
    [ShowIf("type", GameStepType.Level)]
    [SerializeField] private PlayableDirector playableDirector;

    [ShowIf("type", GameStepType.Encounter)]
    [SerializeField] private Encounter encounter;

    public void Play()
    {
        switch (type)
        {
            case GameStepType.Level:
                playableDirector.Play();
                break;
            case GameStepType.Encounter:
                encounter.Play();
                break;
        }
    }
    
    public void Resolve()
    {
        switch (type)
        {
            case GameStepType.Level:
                if (playableDirector)
                {
                    playableDirector.time = playableDirector.duration;
                    playableDirector.Evaluate();
                    playableDirector.Stop();
                }
                break;
            case GameStepType.Encounter:
                if (encounter)
                    encounter.Resolve();
                break;
        }
    }
}