using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config/Fireball", fileName = "New Spell Config")]
public class FireballSpellConfig : SpellConfig
{
    [Header("Burn")] 
    public float burnTime = 10f;
    public float burnDamage = 10f;
    public float burnTick = 1f;
    
    [Header("Fireball")]
    public float launchSpeed = 20f;
    public float knockbackForce = 20f;
    public float knockbackTime = 0.2f;
    public Projectile projectilePrefab;
}
