using UnityEngine;

public class EnemyData
{
    public EnemyConfig config;
    public Material outlineShader;
    public Material pulseShader;

    public EnemyData(EnemyConfig enemyConfig, Material outline, Material pulse)
    {
        config = enemyConfig;
        outlineShader = outline;
        pulseShader = pulse;
    }
}