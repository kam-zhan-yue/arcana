using System;
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
    private CardState _state = CardState.Idle;
    public CardState State => _state;

    public Action<CardPopupItem> BeginDrag;
    public Action<CardPopupItem> EndDrag;
    public Action<CardPopupItem> PointerEnter;
    public Action<CardPopupItem> PointerExit;
    
    private void Update()
    {
        switch (_state)
        {
            case CardState.Idle:
                transform.localPosition = Vector2.zero;
                break;
            case CardState.Dragging:
                Vector2 mousePos = Input.mousePosition;
                transform.position = mousePos;
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
        BeginDrag?.Invoke(this);
        _state = CardState.Dragging;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke(this);
        _state = CardState.Idle;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke(this);
        if (_state == CardState.Idle) 
            _state = CardState.Hovering;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke(this);
        if (_state == CardState.Hovering)
            _state = CardState.Idle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
