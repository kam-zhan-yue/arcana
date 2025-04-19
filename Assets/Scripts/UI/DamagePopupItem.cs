using System.Globalization;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;

public class DamagePopupItem : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text effectText;
    
    public void Init(Damage damage)
    {
        damageText.SetText(damage.Amount.ToString(CultureInfo.InvariantCulture));
        switch (damage.Effect)
        {
            case DamageEffect.None:
                effectText.gameObject.SetActiveFast(false);
                break;
            case DamageEffect.Melt:
                effectText.SetText("Melt");
                break;
            case DamageEffect.Electrocute:
                effectText.SetText("Electrocute");
                break;
        }
    }
}
