using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelSettings
{
    public bool active;
    public string name;
    public int buildIndex;

    public bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(name) == 1;
    }

    public void Unlock()
    {
        PlayerPrefs.SetInt(name, 1);
        PlayerPrefs.Save();
    }
}

[Serializable]
public class StatusSetting
{
    public Status status;
    [ColorUsage(true, true)]
    public Color flashColour;
    public ParticleSystem particles;
    public Vector3 offset;
}

[Serializable]
public class TypeSetting
{
    public DamageType type;
    [ColorUsage(true, true)]
    public Color flashColour;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Game Settings", fileName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    public int playerHealth;
    
    [TableList] public LevelSettings[] levels = Array.Empty<LevelSettings>();

    [TableList]
    public List<StatusSetting> statusParticles = new();

    [TableList]
    public List<TypeSetting> typeSettings = new();
    
    [Header("Pulse")] public float pulseTime = 1f;
    public AnimationCurve pulseCurve;
    [ColorUsage(true, true)] public Color frozenColour;
    public float frozenAlpha;
    
    public int GetNextScene()
    {
        // Find the current level with the active build index
        int currentBuild = SceneManager.GetActiveScene().buildIndex;
        int index = -1;
        for (int i = 0; i < levels.Length; ++i)
        {
            if (levels[i].buildIndex == currentBuild)
            {
                index = i;
                break;
            }
        }

        // If no existing level, don't bother
        if (index < 0)
            return index;

        // Return the next active level in the list
        for (int i = index + 1; i < levels.Length; ++i)
        {
            if (levels[i].active)
            {
                return levels[i].buildIndex;
            }
        }

        return -1;
    }

    public StatusSetting GetParticleByStatus(Status status)
    {
        foreach (StatusSetting statusParticle in statusParticles)
        {
            if (statusParticle.status == status)
                return statusParticle;
        }

        throw new KeyNotFoundException($"Could not find particles for {status}");
    }

    public TypeSetting GetSettingByType(DamageType type)
    {
        foreach (TypeSetting typeSetting in typeSettings)
        {
            if (typeSetting.type == type)
                return typeSetting;
        }

        return null;
    }

    [Button]
    private void UnlockSave()
    {
        foreach(LevelSettings level in levels)
            level.Unlock();
    }

    [Button]
    private void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
}