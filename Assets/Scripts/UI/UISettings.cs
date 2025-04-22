using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/UI Settings", fileName = "UI Settings")]
public class UISettings : ScriptableObject
{
    [Header("UI Parameters")] public Color activateDisabled;
    public Color activateRestricted;
    public Color activateEnabled;
    
    [Header("Follow Parameters")] 
    [SerializeField] public float followSpeed = 25f;

    [Header("Rotation Parameters")] 
    [SerializeField] public float rotationAmount = 1f;
    [SerializeField] public float rotationSpeed = 100f;
    [SerializeField] public float maxRotation = 60f;
    public AnimationCurve positionCurve;
    public float positionInfluence;
    [SerializeField] public AnimationCurve rotationCurve;
    [SerializeField] public float rotationInfluence;

    [Header("Visual Parameters")] [SerializeField]
    public float dragAlpha = 0.4f;

    [Header("Animations")] public float hoverScale = 1.1f;
    public float hoverScaleDuration = 0.1f;
    public float hoverPunchAngle = 10f;
    
    [Header("Outline")]
    public float outlineSize = 0.5f;
    [ColorUsage(true, true)]
    public Color selectColour;
    [ColorUsage(true, true)] public Color idleColour;
    public float cardFadeTime = 0.2f;

    [Header("Tooltip")] public Vector3 tooltipOffset;
}
