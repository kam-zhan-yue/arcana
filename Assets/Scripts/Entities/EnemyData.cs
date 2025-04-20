using UnityEngine;

public class EnemyData
{
    public EnemyConfig config;
    public Material outlineShader;
    public Material pulseShader;
    public bool spawnFromGround;
    public float timeToSpawn;

    public EnemyData(EnemyConfig enemyConfig, Material outline, Material pulse, float spawnTime)
    {
        config = enemyConfig;
        outlineShader = outline;
        pulseShader = pulse;
        timeToSpawn = spawnTime;
    }
}