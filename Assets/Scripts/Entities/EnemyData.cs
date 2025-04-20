using UnityEngine;

public class EnemyData
{
    public EnemyConfig config;
    public Material outlineShader;
    public Material pulseShader;
    public bool spawnFromGround;

    public EnemyData(EnemyConfig enemyConfig, Material outline, Material pulse)
    {
        config = enemyConfig;
        outlineShader = outline;
        pulseShader = pulse;
    }
}