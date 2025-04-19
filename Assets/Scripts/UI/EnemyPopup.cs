using System;
using Kuroneko.UIDelivery;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPopup : Popup
{
    [SerializeField] private DamagePopup damagePopup;
    [FormerlySerializedAs("effectPopup")] [SerializeField] private StatusPopup statusPopup;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    private Enemy _enemy;
    
    protected override void InitPopup()
    {
        _enemy = GetComponentInParent<Enemy>();
        if (!_enemy)
        {
            Debug.LogError("This enemy popup is not attached to an enemy.");
            enabled = false;
        }
        Init();
    }

    private void Init()
    {
        damagePopup.Init(_enemy);
        statusPopup.Init(_enemy);
    }
}
