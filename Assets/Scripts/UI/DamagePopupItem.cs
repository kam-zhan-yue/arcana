using System.Globalization;
using TMPro;
using UnityEngine;

public class DamagePopupItem : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
    
    public void Init(Damage damage)
    {
        _text.SetText(damage.Amount.ToString(CultureInfo.InvariantCulture));
    }
}
