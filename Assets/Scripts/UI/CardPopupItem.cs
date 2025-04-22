using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Idle,
    Dragging,
    Hovering,
}

public class CardPopupItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private Camera _uiCamera;

    private CardState _state = CardState.Idle;
    public CardState State => _state;

    public Action<CardPopupItem> BeginDrag;
    public Action<CardPopupItem> EndDrag;
    public Action<CardPopupItem> PointerEnter;
    public Action<CardPopupItem> PointerExit;

    private RectTransform _rect;
    public RectTransform Rect => _rect;
    public Spell spell;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _uiCamera = Camera.main;
    }

    private void Update()
    {
        switch (_state)
        {
            case CardState.Idle:
                _rect.localPosition = Vector2.zero;
                break;
            case CardState.Dragging:
                Vector3 screenMousePos = Input.mousePosition;
                screenMousePos.z = _uiCamera.WorldToScreenPoint(_rect.position).z;
                Vector3 worldMousePos = _uiCamera.ScreenToWorldPoint(screenMousePos);
                _rect.position = worldMousePos;
                break;
        }
    }

    public int GetParentIndex()
    {
        return transform.parent.GetSiblingIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _state = CardState.Dragging;
        BeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _state = CardState.Idle;
        EndDrag?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_state == CardState.Idle)
        {
            _state = CardState.Hovering;
            PointerEnter?.Invoke(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_state == CardState.Hovering)
        {
            _state = CardState.Idle;
            PointerExit?.Invoke(this);
        }
    }

    public float GetNormalizedPosition()
    {
        if (transform.parent.parent.childCount == 0)
            return 0;
        if (transform.parent.parent.childCount == 1)
            return 0.5f;
        float parentIndex = (float)GetParentIndex();
        float remap = parentIndex.Remap(0f, transform.parent.parent.childCount - 1, 0f, 1f);
        return remap;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
