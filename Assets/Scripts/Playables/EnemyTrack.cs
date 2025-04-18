using UnityEngine.Timeline;

[TrackColor(1, 0.5f, 0.5f)]
[TrackBindingType(typeof(Enemy))]
[TrackClipType(typeof(BasicMovementClip))]
[TrackClipType(typeof(JumpMovementClip))]
[TrackClipType(typeof(AttackMeleeClip))]
public class EnemyTrack : TrackAsset
{
    
}
