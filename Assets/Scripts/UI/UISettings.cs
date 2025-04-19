using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class TypeSetting
{
    public DamageType type;
    public Color colour;
}

[Serializable]
public class EffectSetting
{
    public DamageEffect effect;
    public Color colour;
}

[CreateAssetMenu(menuName = "ScriptableObjects/UI Settings", fileName = "UI Settings")]
public class UISettings : ScriptableObject
{
    public float outlineSize = 0.5f;
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
}
