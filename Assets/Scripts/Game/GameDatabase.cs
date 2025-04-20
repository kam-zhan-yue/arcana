using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Game Database", fileName = "Game Database")]
public class GameDatabase : ScriptableObject
{
    public EnemyDatabase enemyDatabase;
    public CardDatabase cardDatabase;
}