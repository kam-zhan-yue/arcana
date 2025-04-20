using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config/Flame Wave", fileName = "New Spell Config")]
public class FlameWaveSpellConfig : SpellConfig
{
    [Header("Burn")] 
    public float burnTime = 10f;
    public float burnDamage = 10f;
    public float burnTick = 1f;
    public float knockbackForce = 1f;
}
