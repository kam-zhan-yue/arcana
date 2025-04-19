using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config/Fireball", fileName = "New Spell Config")]
public class FireballSpellConfig : SpellConfig
{
    public float launchSpeed = 20f;
    public float knockbackForce = 20f;
    public float knockbackTime = 0.2f;
    public FireballProjectile fireballPrefab;
}
