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
public struct EnemyActivationData
{
    [HideLabel, HorizontalGroup(Width = 0.5f)]
    public Enemy enemy;
    
    [HideLabel, HorizontalGroup(Width = 0.5f)]
    public EnemyConfig config;
}

[Serializable]
public struct EnemySpawnData
{
    [HideLabel, HorizontalGroup(Width = 0.5f)]
    public Transform spawnPoint;

    [HideLabel, HorizontalGroup(Width = 0.5f)]
    public Enemy enemyPrefab;
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
    public List<EnemyActivationData> enemiesToActivate;
    
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
                    enemiesToActivate[i].enemy.Init(enemiesToActivate[i].config);
                    enemiesToActivate[i].enemy.Activate();
                    encounter.enemies.Add(enemiesToActivate[i].enemy);
                }
                break;
            case EncounterStepType.Spawn:
                for (int i = 0; i < spawnData.Count; ++i)
                {
                    Enemy enemy = Object.Instantiate(spawnData[i].enemyPrefab, spawnData[i].spawnPoint);
                    encounter.enemies.Add(enemy);
                }
                break;
            case EncounterStepType.Wait:
                await UniTask.WaitUntil(() => encounter.enemies.Count == 0);
                break;
        }
    }
    
    
    [ShowIf("type", EncounterStepType.Spawn)]
    [HorizontalGroup()]
    [Button((ButtonSizes.Small)), GUIColor(0.2f, 1f, 0)]
    public void AddSpawnData()
    {
        spawnData.Add(new EnemySpawnData());
    }
}
