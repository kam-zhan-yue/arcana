using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Archer Config", fileName = "Enemy Archer")]
public class EnemyArcherConfig : EnemyConfig
{
    public float startupTime = 2f;
    public ProjectileEnemy arrowPrefab;
    public float arrowSpeed = 5f;
}
