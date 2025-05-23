using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config/Water Bucket", fileName = "New Spell Config")]
public class WaterBucketSpellConfig : SpellConfig
{
    public float wetTime = 5f;
    public Projectile projectile;
    public ParticleSystem splashEffect;
    public float knockbackForce;
    public float launchSpeed;
}
