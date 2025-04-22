using System;
using Kuroneko.UIDelivery;
using TMPro;
using UnityEngine;

public class TooltipPopup : Popup
{
    [SerializeField] private CardDatabase cardDatabase;
    [SerializeField] private UISettings settings;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private RectTransform _rect;
    private Camera _uiCamera;
    private bool _canShow = true;
    
    protected override void InitPopup()
    {
        _rect = GetComponent<RectTransform>();
        _uiCamera = Camera.main;
        HidePopup();
    }

    private void Update()
    {
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = _uiCamera.WorldToScreenPoint(_rect.position).z;
        Vector3 worldMousePos = _uiCamera.ScreenToWorldPoint(screenMousePos);
        _rect.position = worldMousePos + settings.tooltipOffset;
    }

    public void Init(CardType type)
    {
        if (!_canShow)
            return;
        Card card = cardDatabase.GetCard(type);
        title.SetText(card.spellConfig.title);
        description.SetText(card.spellConfig.description);
        ShowPopup();
    }

    public void Disable()
    {
        _canShow = false;
    }

    public void Enable()
    {
        _canShow = true;
    }

    public void Hide()
    {
        HidePopup();
    }
}