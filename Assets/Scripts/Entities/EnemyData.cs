using UnityEngine;

public class EnemyData
{
    public readonly EnemyConfig config;
    public readonly Material outlineShader;
    public readonly Material pulseShader;
    public readonly Material frozenShader;
    public bool spawnFromGround;
    public readonly float timeToSpawn;

    public EnemyData(EnemyConfig enemyConfig, Material outline, Material pulse, Material frozen, float spawnTime)
    {
        config = enemyConfig;
        outlineShader = outline;
        pulseShader = pulse;
        frozenShader = frozen;
        timeToSpawn = spawnTime;
    }
}