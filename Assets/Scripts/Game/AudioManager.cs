using Kuroneko.AudioDelivery;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundDatabase soundDatabase;
    public static AudioManager instance;
    private bool _playingMain = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayStartBGM()
    {
        Stop("BGM_MAIN");
        Play("BGM_START");
    }

    public void PlayMainBGM()
    {
        if (_playingMain)
            return;
        _playingMain = true;
        Stop("BGM_MAIN");
        Stop("BGM_START");
        Play("BGM_MAIN");
    }

    public void Play(string clipName, string instanceId = "")
    {
        if (soundDatabase.TryGetSound(clipName, out Sound sound))
            sound.config.Play(instanceId);
        else
            Debug.LogWarning($"AudioDelivery | {clipName} could not be found in {soundDatabase.name}!");
    }

    public void Pause(string clipName, string instanceId = "")
    {
        if (soundDatabase.TryGetSound(clipName, out Sound sound))
            sound.config.Pause(instanceId);
        else
            Debug.LogWarning($"AudioDelivery | {clipName} could not be found in {soundDatabase.name}!");
    }

    public void Resume(string clipName, string instanceId = "")
    {
        if (soundDatabase.TryGetSound(clipName, out Sound sound))
            sound.config.Resume(instanceId);
        else
            Debug.LogWarning($"AudioDelivery | {clipName} could not be found in {soundDatabase.name}!");
    }

    public void Stop(string clipName, string instanceId = "")
    {
        if (soundDatabase.TryGetSound(clipName, out Sound sound))
            sound.config.Stop(instanceId);
        else
            Debug.LogWarning($"AudioDelivery | {clipName} could not be found in {soundDatabase.name}!");
    }
}
