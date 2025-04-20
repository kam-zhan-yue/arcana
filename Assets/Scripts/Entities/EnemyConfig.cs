using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Config", fileName = "New Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    public Enemy prefab;
    public float moveSpeed = 10f;
    public float maxHealth = 100f;
    public float attackRange = 1f;
    public float timeBetweenAttacks = 1f;
    public float attackAnimationTime = 0.2f;
    public float startupTime = 1f;
}
