using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Mage Config", fileName = "Enemy Mage")]
public class EnemyMageConfig : EnemyConfig
{
    public ProjectileEnemy spellPrefab;
    public float spellSpeed = 2f;
}