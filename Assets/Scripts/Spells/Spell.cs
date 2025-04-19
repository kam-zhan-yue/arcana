using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header("Settings")] [SerializeField] protected UISettings settings;
    [Header("Follow Parameters")] 
    [SerializeField] private float followSpeed = 25f;

    [Header("Rotation Parameters")] 
    [SerializeField] private float rotationAmount = 1f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float maxRotation = 60f;
    [SerializeField] private float tiltSpeed = 20f;

    protected float damage;
    protected float interactingTime = 0f;
    protected ISpellTarget target;
    private CardPopupItem _cardPopupItem;
    private Vector3 _movementDelta;
    private Vector3 _rotationDelta;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    public void Init(SpellConfig config, CardPopupItem cardPopupItem)
    {
        damage = config.damage;
        _cardPopupItem = cardPopupItem;
        _cardPopupItem.EndDrag += OnEndDrag;
        _cardPopupItem.PointerEnter += OnPointerEnter;
        _cardPopupItem.PointerExit += OnPointerExit;
    }

    private void Update()
    {
        if (!_cardPopupItem) return;
        Follow();
        Rotate();
        CheckTarget();

        if (_cardPopupItem.State == CardState.Hovering || _cardPopupItem.State == CardState.Dragging)
        {
            OnInteracting();
        }
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, _cardPopupItem.transform.position, followSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 movementVector = (transform.position - _cardPopupItem.transform.position);
        _movementDelta = Vector3.Lerp(_movementDelta, movementVector, 25 * Time.deltaTime);
        Vector3 movementRotation = (_cardPopupItem.State == CardState.Dragging ? _movementDelta : movementVector) * rotationAmount;
        _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(_rotationDelta.x, -maxRotation, maxRotation));
    }

    private void CheckTarget()
    {
        if (_cardPopupItem.State != CardState.Dragging)
            return;
        target = null;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out ISpellTarget spellTarget))
            {
                target = spellTarget;
            }
        }
    }
    
    private void OnEndDrag(CardPopupItem cardPopupItem)
    {
        OnStopInteracting();
        if (target != null)
            Apply(target);
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

    protected abstract void Apply(ISpellTarget spellTarget);

    private void OnDestroy()
    {
        _cardPopupItem.EndDrag -= OnEndDrag;
        _cardPopupItem.PointerEnter -= OnPointerEnter;
        _cardPopupItem.PointerExit -= OnPointerExit;
    }
}
