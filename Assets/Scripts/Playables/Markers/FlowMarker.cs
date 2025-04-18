using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FlowMarker : Marker, INotification, INotificationOptionProvider
{
    public PropertyName id => new();

    // Do not set this retroactive as we want to be able to skip these.
    public NotificationFlags flags => NotificationFlags.TriggerOnce;
}