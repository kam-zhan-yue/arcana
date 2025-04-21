using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Database", fileName = "Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public Material outlineShader;
    public Material pulseShader;
    public float lightIntensity = 1f;
    public float lightTweenDuration = 2f;
    public float timeToSpawn = 3f;
    [InlineEditor]
    public List<EnemyConfig> configs = new();

    public EnemyData GetDataByEnemy(Enemy enemy)
    {
        Type enemyType = enemy.GetType();
        foreach (EnemyConfig config in configs)
        {
            if (config.prefab.GetType() == enemyType)
            {
                return CreateData(config);
            }
        }
        throw new KeyNotFoundException($"Could not find a config that matches {enemy}");
    }

    public EnemyData GetDataByConfig(EnemyConfig config)
    {
        return CreateData(config);
    }

    private EnemyData CreateData(EnemyConfig config)
    {
        EnemyData data = new(config, outlineShader, pulseShader, timeToSpawn);
        return data;
    }
}