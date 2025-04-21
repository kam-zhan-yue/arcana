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
public class StatusParticle
{
    public Status status;
    public ParticleSystem particles;
    public Vector3 offset;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Game Settings", fileName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    public int playerHealth;
    
    [TableList] public LevelSettings[] levels = Array.Empty<LevelSettings>();

    public List<StatusParticle> statusParticles = new();
    
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

    public StatusParticle GetParticleByStatus(Status status)
    {
        foreach (StatusParticle statusParticle in statusParticles)
        {
            if (statusParticle.status == status)
                return statusParticle;
        }

        throw new KeyNotFoundException($"Could not find particles for {status}");
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