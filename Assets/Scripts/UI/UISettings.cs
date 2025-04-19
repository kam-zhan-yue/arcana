using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class TypeSetting
{
    public DamageType type;
    [ColorUsage(true, true)]
    public Color colour;
}

[Serializable]
public class EffectSetting
{
    public DamageEffect effect;
    [ColorUsage(true, true)]
    public Color colour;
}

[CreateAssetMenu(menuName = "ScriptableObjects/UI Settings", fileName = "UI Settings")]
public class UISettings : ScriptableObject
{
    [Header("Follow Parameters")] 
    [SerializeField] public float followSpeed = 25f;

    [Header("Rotation Parameters")] 
    [SerializeField] public float rotationAmount = 1f;
    [SerializeField] public float rotationSpeed = 100f;
    [SerializeField] public float maxRotation = 60f;
    
    [Header("Outline")]
    public float outlineSize = 0.5f;
    [ColorUsage(true, true)]
    public Color selectColour;

    [Header("Pulse")] public float pulseTime = 1f;
    public AnimationCurve pulseCurve;
    
    [TableList] public List<TypeSetting> typeSettings = new();
    [TableList] public List<EffectSetting> effectSettings = new();

    public TypeSetting GetSettingForType(DamageType type)
    {
        for (int i = 0; i < typeSettings.Count; ++i)
        {
            if (type == typeSettings[i].type)
                return typeSettings[i];
        }

        throw new KeyNotFoundException($"No TypeSetting found for type {type}");
    }
    
    public EffectSetting GetSettingForEffect(DamageEffect effect)
    {
        for (int i = 0; i < effectSettings.Count; ++i)
        {
            if (effect == effectSettings[i].effect)
                return effectSettings[i];
        }

        throw new KeyNotFoundException($"No TypeSetting found for type {effect}");
    }

    public float GetPulseAmount(float time)
    {
        float t = time / pulseTime;
        t = Mathf.Repeat(t, 1f);
        return pulseCurve.Evaluate(t);
    }
}
