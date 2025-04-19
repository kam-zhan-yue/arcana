using System;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    private Fireball _fireball;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Fireball fireball)
    {
        _fireball = fireball;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            _fireball.ApplyEnemy(enemy, this);
        }
    }
}
