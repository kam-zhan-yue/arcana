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

    public int GetSiblingIndex()
    {
        return transform.parent.parent.childCount - 1;
    }

    public float GetNormalizedPosition()
    {
        return ((float)GetParentIndex()).Remap(0, (float)(transform.parent.parent.childCount - 1), 0,
            1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
