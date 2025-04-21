using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config/Lightning", fileName = "Lightning")]
public class LightningSpellConfig : SpellConfig
{
    public float knockbackForce = 0.5f;
    public ParticleSystem lightingEffect;
}
