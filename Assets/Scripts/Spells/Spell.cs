using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header("Settings")] [SerializeField] protected UISettings settings;
    // [SerializeField] private float tiltSpeed = 20f;

    protected float damage;
    protected float interactingTime = 0f;
    protected CardPopup cardPopup;
    private CardPopupItem _cardPopupItem;
    private Vector3 _movementDelta;
    private Vector3 _rotationDelta;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    public void Init(SpellConfig config, CardPopupItem cardPopupItem, CardPopup popup)
    {
        damage = config.damage;
        _cardPopupItem = cardPopupItem;
        _cardPopupItem.BeginDrag += OnBeginDrag;
        _cardPopupItem.EndDrag += OnEndDrag;
        _cardPopupItem.PointerEnter += OnPointerEnter;
        _cardPopupItem.PointerExit += OnPointerExit;
        cardPopup = popup;
    }

    private void Update()
    {
        if (!_cardPopupItem) return;
        Follow();
        Rotate();

        if (_cardPopupItem.State == CardState.Hovering || _cardPopupItem.State == CardState.Dragging)
        {
            OnInteracting();
        }
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, _cardPopupItem.transform.position, settings.followSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 movementVector = (transform.position - _cardPopupItem.transform.position);
        _movementDelta = Vector3.Lerp(_movementDelta, movementVector, 25 * Time.deltaTime);
        Vector3 movementRotation = (_cardPopupItem.State == CardState.Dragging ? _movementDelta : movementVector) * settings.rotationAmount;
        _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, settings.rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(_rotationDelta.x, -settings.maxRotation, settings.maxRotation));
    }

    protected Enemy GetCurrentTarget()
    {
        Enemy targetedEnemy = null;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out Enemy spellTarget))
            {
                targetedEnemy = spellTarget;
            }
        }

        return targetedEnemy;
    }

    private void OnBeginDrag(CardPopupItem cardPopupItem)
    {
        OnStartDragging();
    }
    
    private void OnEndDrag(CardPopupItem cardPopupItem)
    {
        List<Enemy> targets = GetTargets();
        for (int i = 0; i < targets.Count; ++i)
            Apply(targets[i]);
        OnStopInteracting();
    }

    private void OnPointerEnter(CardPopupItem cardPopupItem)
    {
        interactingTime = 0f;
        OnStartInteracting();
    }
    
    private void OnPointerExit(CardPopupItem cardPopupItem)
    {
        OnStopInteracting();
    }

    protected virtual void OnStartDragging()
    {
        
    }

    protected virtual void OnStartInteracting()
    {
        interactingTime = 0f;
    }

    protected virtual void OnInteracting()
    {
        interactingTime += Time.deltaTime;
    }

    protected virtual void OnStopInteracting()
    {
        interactingTime = 0f;
    }

    protected abstract List<Enemy> GetTargets();

    protected abstract void Apply(Enemy spellTarget);

    private void OnDestroy()
    {
        _cardPopupItem.EndDrag -= OnEndDrag;
        _cardPopupItem.PointerEnter -= OnPointerEnter;
        _cardPopupItem.PointerExit -= OnPointerExit;
    }
}
