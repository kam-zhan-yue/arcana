using UnityEditor;
using UnityEngine;

public abstract class SpellConfig : ScriptableObject
{
    public Spell prefab;
    public float damage;
}
