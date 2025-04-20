using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Game Database", fileName = "Game Database")]
public class GameDatabase : ScriptableObject
{
    [InlineEditor]
    public EnemyDatabase enemyDatabase;
    [InlineEditor]
    public CardDatabase cardDatabase;
}