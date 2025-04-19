using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header("Follow Parameters")] 
    [SerializeField] private float followSpeed = 25f;

    [Header("Rotation Parameters")] 
    [SerializeField] private float rotationAmount = 1f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float maxRotation = 60f;
    [SerializeField] private float tiltSpeed = 20f;

    protected float damage;
    private CardPopupItem _cardPopupItem;
    private Vector3 _movementDelta;
    private Vector3 _rotationDelta;
    private Camera _mainCamera;
    private ISpellTarget _target;

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
        _target = null;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out ISpellTarget spellTarget))
            {
                _target = spellTarget;
            }
        }
    }
    
    private void OnEndDrag(CardPopupItem cardPopupItem)
    {
        if (_target != null)
            Apply(_target);
    }

    private void OnPointerEnter(CardPopupItem cardPopupItem)
    {
        Hover();
    }
    
    private void OnPointerExit(CardPopupItem cardPopupItem)
    {
        UnHover();
    }

    protected abstract void Apply(ISpellTarget target);
    protected abstract void Hover();
    protected abstract void UnHover();


    private void OnDestroy()
    {
        _cardPopupItem.EndDrag -= OnEndDrag;
        _cardPopupItem.PointerEnter -= OnPointerEnter;
        _cardPopupItem.PointerExit -= OnPointerExit;
    }
}
