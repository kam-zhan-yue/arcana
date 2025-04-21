using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kuroneko.UIDelivery;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamagePopup : Popup
{
    [SerializeField] private RectTransform damageHolder;
    [SerializeField] private DamagePopupItem sampleDamagePopupItem;
    
    protected override void InitPopup()
    {
    }

    public void Init(Enemy enemy)
    {
        enemy.OnDamage += OnDamage;
    }

    [Button]
    private void DebugRandomDamage()
    {
        Damage damage = new(Random.Range(0f, 100f), DamageType.Basic, DamageEffect.None);
        OnDamage(damage);
    }

    private void OnDamage(Damage damage)
    {
        AddDamageAsync(damage).Forget();
    }

    private async UniTask AddDamageAsync(Damage damage)
    {
        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();
        DamagePopupItem popupItem = Instantiate(sampleDamagePopupItem, damageHolder);
        popupItem.Init(damage);
        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: cancellationToken);
        Destroy(popupItem.gameObject);
    }
}
