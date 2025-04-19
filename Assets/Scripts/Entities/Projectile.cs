using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    private IProjectileSpell _spell;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(IProjectileSpell spell)
    {
        _spell = spell;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            _spell.ApplyEnemy(enemy, this);
        }
    }
}