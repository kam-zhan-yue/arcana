using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Spell Config", fileName = "New Spell Config")]
public class SpellConfig : ScriptableObject
{
    public Spell prefab;
    public float damage;
}
