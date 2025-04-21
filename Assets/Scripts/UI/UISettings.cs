using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UI Settings", fileName = "UI Settings")]
public class UISettings : ScriptableObject
{
    [Header("UI Parameters")] public Color activateDisabled;
    public Color activateEnabled;
    
    [Header("Follow Parameters")] 
    [SerializeField] public float followSpeed = 25f;

    [Header("Rotation Parameters")] 
    [SerializeField] public float rotationAmount = 1f;
    [SerializeField] public float rotationSpeed = 100f;
    [SerializeField] public float maxRotation = 60f;

    [Header("Visual Parameters")] [SerializeField]
    public float dragAlpha = 0.4f;
    
    [Header("Outline")]
    public float outlineSize = 0.5f;
    [ColorUsage(true, true)]
    public Color selectColour;
    [ColorUsage(true, true)] public Color idleColour;
    public float cardFadeTime = 0.2f;
}
