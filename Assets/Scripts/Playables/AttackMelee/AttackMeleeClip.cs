using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AttackMeleeClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private AttackMeleeBehaviour template = new();
    
    public override double duration => 1.0;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<AttackMeleeBehaviour>.Create(graph, template);
    }
}
