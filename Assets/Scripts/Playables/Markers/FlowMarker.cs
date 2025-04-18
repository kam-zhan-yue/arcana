using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FlowMarker : Marker, INotification
{
    public PropertyName id => new();
}