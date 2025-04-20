using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera headCamera;
    [SerializeField] private Transform launchPosition;
    
    public Transform GetLaunchPosition()
    {
        return launchPosition;
    }

    public Transform GetHead()
    {
        return headCamera.transform;
    }

    public void Heal()
    {
        
    }

    public void Damage()
    {
        Debug.Log("Damage");
    }
}

