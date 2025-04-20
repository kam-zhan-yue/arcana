using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public enum EncounterStepType
{
    Spawn,
    Activate,
    Delay,
    Wait,
}

[Serializable]
public struct EnemySpawnData
{
    [HideLabel, HorizontalGroup(Width = 0.5f)]
    public Transform spawnPoint;

    [HideLabel, HorizontalGroup(Width = 0.5f)]
    public EnemyConfig enemyConfig;
}

[Serializable]
public struct EncounterStep
{
    [HideLabel] public EncounterStepType type;

    [HideLabel, ShowIf("type", EncounterStepType.Delay)] [SerializeField]
    public float delayTime;

    [HideLabel, ShowIf("type", EncounterStepType.Spawn), TableList] [SerializeField]
    public List<EnemySpawnData> spawnData;

    [ShowIf("type", EncounterStepType.Activate)] [SerializeField]
    public List<Enemy> enemiesToActivate;
    
    public async UniTask Play(Encounter encounter)
    {
        await UniTask.NextFrame();
        switch (type)
        {
            case EncounterStepType.Delay:
                await UniTask.WaitForSeconds(delayTime);
                break;
            case EncounterStepType.Activate:
                Debug.Log("Activating");
                for (int i = 0; i < enemiesToActivate.Count; ++i)
                {
                    EnemyDatabase enemyDatabase = GetEnemyDatabase();
                    EnemyData data = enemyDatabase.GetDataByEnemy(enemiesToActivate[i]);
                    enemiesToActivate[i].Init(data);
                    enemiesToActivate[i].Activate();
                    encounter.AddEnemy(enemiesToActivate[i]);
                }
                break;
            case EncounterStepType.Spawn:
                for (int i = 0; i < spawnData.Count; ++i)
                {
                    Enemy enemy = Object.Instantiate(spawnData[i].enemyConfig.prefab, spawnData[i].spawnPoint);
                    EnemyDatabase enemyDatabase = GetEnemyDatabase();
                    EnemyData data = enemyDatabase.GetDataByConfig(spawnData[i].enemyConfig);
                    enemy.Init(data);
                    encounter.AddEnemy(enemy);
                }
                break;
            case EncounterStepType.Wait:
                await UniTask.WaitUntil(() => encounter.enemies.Count == 0);
                break;
        }
    }

    private EnemyDatabase GetEnemyDatabase()
    {
        GameDatabase gameDatabase = ServiceLocator.Instance.Get<IGameManager>().GetGameDatabase();
        EnemyDatabase enemyDatabase = gameDatabase.enemyDatabase;
        return enemyDatabase;
    }
    
    [ShowIf("type", EncounterStepType.Spawn)]
    [HorizontalGroup()]
    [Button((ButtonSizes.Small)), GUIColor(0.2f, 1f, 0)]
    public void AddSpawnData()
    {
        spawnData.Add(new EnemySpawnData());
    }
}
