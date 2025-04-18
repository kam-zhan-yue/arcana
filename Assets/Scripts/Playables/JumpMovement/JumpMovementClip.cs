using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class JumpMovementClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private JumpMovementBehaviour template = new();
    
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<JumpMovementBehaviour>.Create(graph, template);
    }
}