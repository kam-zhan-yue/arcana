using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config/Snowball", fileName = "Snowball")]
public class SnowballSpellConfig : SpellConfig
{
    [Header("Freeze")] 
    public float freezeTime = 10f;
    
    [Header("Snowball")]
    public float launchSpeed = 5f;
    public float knockbackForce = 20f;
    public float knockbackTime = 0.2f;
    public Projectile projectilePrefab;
    public ParticleSystem explosionEffect;
}
