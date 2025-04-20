using UnityEngine;
using UnityEngine.Playables;

public class AttackMeleeBehaviour : PlayableBehaviour
{
    private bool _hasAttacked = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Enemy enemy = playerData as Enemy;
        if (enemy == null)
            return;

        if (_hasAttacked)
            return;

        _hasAttacked = true;
    }
    
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        _hasAttacked = false; 
    }
}
