using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform launchPosition;
    
    public Transform GetLaunchPosition()
    {
        return launchPosition;
    }
}

