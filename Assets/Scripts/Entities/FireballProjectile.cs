using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
