using UnityEngine;

public class EnemyData
{
    public readonly EnemyConfig config;
    public readonly Material outlineShader;
    public readonly Material pulseShader;
    public bool spawnFromGround;
    public readonly float timeToSpawn;

    public EnemyData(EnemyConfig enemyConfig, Material outline, Material pulse, float spawnTime)
    {
        config = enemyConfig;
        outlineShader = outline;
        pulseShader = pulse;
        timeToSpawn = spawnTime;
    }
}