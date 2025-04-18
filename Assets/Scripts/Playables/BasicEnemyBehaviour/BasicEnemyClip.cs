using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BasicEnemyClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private BasicEnemyBehaviour template = new BasicEnemyBehaviour();
    
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<BasicEnemyBehaviour>.Create(graph, template);
    }
}
