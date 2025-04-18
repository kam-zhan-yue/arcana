using System;
using Cysharp.Threading.Tasks;
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
    [HideLabel]
    public GameStepType type;
    
    [HideLabel, ShowIf("type", GameStepType.Level)]
    [SerializeField] private PlayableDirector playableDirector;

    [HideLabel, ShowIf("type", GameStepType.Encounter)]
    [SerializeField] private Encounter encounter;

    public async UniTask Play()
    {
        switch (type)
        {
            case GameStepType.Level:
                playableDirector.Play();
                await UniTask.WaitUntil(() => playableDirector.state != PlayState.Playing);
                break;
            case GameStepType.Encounter:
                await encounter.Play();
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
                if (encounter != null)
                    encounter.Resolve();
                break;
        }
    }
}