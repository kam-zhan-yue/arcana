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
public struct EnemyActivateData
{
    public Enemy enemy;
    public bool spawnFromGround;
}

[Serializable]
public struct EnemySpawnData
{
    public EnemyConfig enemyConfig;
    public Transform spawnPoint;
    public bool spawnFromGround;
}

[Serializable]
public struct EncounterStep
{
    [HideLabel] public EncounterStepType type;

    [HideLabel, ShowIf("type", EncounterStepType.Delay)] [SerializeField]
    public float delayTime;

    [ShowIf("type", EncounterStepType.Spawn), TableList] [SerializeField]
    public List<EnemySpawnData> spawnData;

    [ShowIf("type", EncounterStepType.Activate), TableList] [SerializeField]
    public List<EnemyActivateData> enemiesToActivate;
    
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
                    EnemyData data = enemyDatabase.GetDataByEnemy(enemiesToActivate[i].enemy);
                    data.spawnFromGround = enemiesToActivate[i].spawnFromGround;
                    enemiesToActivate[i].enemy.Init(data);
                    encounter.AddEnemy(enemiesToActivate[i].enemy);
                }
                break;
            case EncounterStepType.Spawn:
                for (int i = 0; i < spawnData.Count; ++i)
                {
                    Enemy enemy = Object.Instantiate(spawnData[i].enemyConfig.prefab, spawnData[i].spawnPoint);
                    EnemyDatabase enemyDatabase = GetEnemyDatabase();
                    EnemyData data = enemyDatabase.GetDataByConfig(spawnData[i].enemyConfig);
                    data.spawnFromGround = spawnData[i].spawnFromGround;
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
        return ServiceLocator.Instance.Get<IGameManager>().GetGame().Database.enemyDatabase;
    }
    
    [ShowIf("type", EncounterStepType.Spawn)]
    [HorizontalGroup()]
    [Button((ButtonSizes.Small)), GUIColor(0.2f, 1f, 0)]
    public void AddSpawnData()
    {
        spawnData.Add(new EnemySpawnData());
    }
}
