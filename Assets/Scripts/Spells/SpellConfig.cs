using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SpellConfig : ScriptableObject
{
    public string title;
    [HideLabel, TextArea]
    public string description;
    public Spell prefab;
    public float damage;
    public DamageType type;
    public bool oneTimeUse;
}
