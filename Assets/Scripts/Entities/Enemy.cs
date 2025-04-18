using UnityEngine;

public class Enemy : MonoBehaviour, ISpellTarget
{
    public Transform GetTransform()
    {
        return transform;
    }
}
