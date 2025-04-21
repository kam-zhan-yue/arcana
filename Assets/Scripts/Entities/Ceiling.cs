using UnityEngine;

public class Ceiling : MonoBehaviour
{
    private void Awake()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }
}
