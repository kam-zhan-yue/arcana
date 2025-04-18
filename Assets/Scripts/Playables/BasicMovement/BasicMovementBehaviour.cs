using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class BasicMovementBehaviour : PlayableBehaviour
{
    [SerializeField] private Transform target;

    private Enemy _enemy;
    private bool _firstFrame;
    private Vector3 _startPosition;
    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Enemy enemy = playerData as Enemy;
        
        if (enemy == null || target == null)
            return;
        

        if (!_firstFrame)
        {
            _enemy = enemy;
            _startPosition = enemy.transform.position;
            _firstFrame = true;
        }

        // Calculate how far through the playable we are (0 to 1)
        double normalizedTime = playable.GetTime() / playable.GetDuration();
        normalizedTime = Math.Clamp(normalizedTime, 0.0, 1.0);

        // Interpolate position from start to target
        Vector3 newPosition = Vector3.Lerp(_startPosition, target.position, (float)normalizedTime);
        enemy.transform.position = newPosition;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (_enemy == null)
        {
            return;
        }

        _enemy.transform.position = _startPosition;
        base.OnBehaviourPause(playable, info );
    }
}

