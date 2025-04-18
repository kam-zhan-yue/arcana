using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Idle,
    Dragging,
}

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform _rectTransform;
    private CardState _state = CardState.Idle;

    public Action<Card> BeginDrag;
    public Action<Card> EndDrag;

    private VisualCard _visualCard;
    
    private void Awake()
    { 
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(VisualCard visualCard)
    {
        _visualCard = visualCard;
    }

    private void Update()
    {
        switch (_state)
        {
            case CardState.Idle:
                _rectTransform.localPosition = Vector2.zero;
                break;
            case CardState.Dragging:
                Vector2 mousePos = Input.mousePosition;
                _rectTransform.position = mousePos;
                break;
        }
        
        _visualCard.MoveTo(_rectTransform.position);
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
