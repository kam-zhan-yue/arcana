using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BasicMovementClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private BasicMovementBehaviour template = new BasicMovementBehaviour();
    
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<BasicMovementBehaviour>.Create(graph, template);
    }
}
