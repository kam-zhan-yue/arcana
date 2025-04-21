using System;
using Cysharp.Threading.Tasks;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

[Serializable]
public enum GameStepType
{
    Level,
    Encounter,
    End,
}

[Serializable]
public class GameStep
{
    [FoldoutGroup("$GroupTitle"), HideLabel]
    public GameStepType type;

    [FoldoutGroup("$GroupTitle"), HideLabel, ShowIf("type", GameStepType.Level)]
    [SerializeField] private PlayableDirector playableDirector;

    [FoldoutGroup("$GroupTitle"), HideLabel, ShowIf("type", GameStepType.Encounter)]
    [SerializeField] private Encounter encounter;

    public string GroupTitle => type.ToString();
    public async UniTask Play()
    {
        switch (type)
        {
            case GameStepType.Level:
                Debug.Log("Playing Playable Director");
                playableDirector.Play();
                await UniTask.WaitUntil(() => playableDirector.state != PlayState.Playing);
                break;
            case GameStepType.Encounter:
                Debug.Log("Playing Encounter");
                await encounter.Play();
                break;
            case GameStepType.End:
                Debug.Log("End Level");
                Game game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
                int next = game.Database.settings.GetNextScene();
                if (next >= 0)
                    SceneManager.LoadScene(next);
                break;
        }
    }
    
    public void Resolve()
    {
        switch (type)
        {
            case GameStepType.Level:
                if (playableDirector)
                    ResolvePlayableDirector();
                break;
            case GameStepType.Encounter:
                if (encounter != null)
                    encounter.Resolve();
                break;
        }
    }

    private void ResolvePlayableDirector()
    {
        TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
        if (timelineAsset == null)
        {
            Debug.LogError("PlayableDirector doesn't have a valid TimelineAsset.");
            return;
        }

        foreach (TrackAsset track in timelineAsset.GetOutputTracks())
        {
            foreach (IMarker marker in track.GetMarkers())
            {
                if (playableDirector.time < marker.time && marker is FlowMarker)
                {
                    // Set the time slightly past the marker so as to not trigger the pause
                    playableDirector.time = marker.time + 0.01f;
                    playableDirector.Evaluate();
                    playableDirector.Pause();
                    Debug.Log($"Resolved Playable to {marker.time}");
                    return;
                }
            }
        }

    }
}