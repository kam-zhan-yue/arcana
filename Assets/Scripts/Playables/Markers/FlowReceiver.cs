using UnityEngine;
using UnityEngine.Playables;

public class FlowReceiver : MonoBehaviour, INotificationReceiver
{
    private PlayableDirector _playableDirector;

    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is FlowMarker)
        {
            Debug.Log("Pause");
            _playableDirector.Pause();
        }
    }
}