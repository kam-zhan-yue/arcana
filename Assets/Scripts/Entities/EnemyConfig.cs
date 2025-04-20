using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Config", fileName = "New Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    public Enemy prefab;
    public float moveSpeed = 10f;
    public float maxHealth = 100f;
}
