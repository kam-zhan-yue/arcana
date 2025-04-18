using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Config", fileName = "New Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    public Enemy prefab;
    public float maxHealth = 100f;

    public Enemy Spawn(Transform parent)
    {
        Enemy enemy = Instantiate(prefab, parent);
        enemy.Init(this);
        return enemy;
    }
}
