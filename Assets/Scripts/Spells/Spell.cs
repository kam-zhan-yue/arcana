using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spell : MonoBehaviour
{
    private const int SHOW_SORT = 100;
    protected UISettings settings;
    // [SerializeField] private float tiltSpeed = 20f;

    protected float damage;
    protected DamageType type;
    protected float interactingTime = 0f;
    protected CardPopup cardPopup;
    private CardPopupItem _cardPopupItem;
    private Vector3 _movementDelta;
    private Vector3 _rotationDelta;
    private Camera _mainCamera;
    private bool _oneTimeUse;
    private CardType _cardType;
    private RectTransform _rect;
    private Image _image;
    private Color _originalColour;
    private Canvas _canvas;

    private float _curveYOffset;
    private float _curveRotationOffset;
    private Tween _scaleTween;
    private Tween _shakeTween;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _rect = GetComponent<RectTransform>();
        // _canvas = GetComponent<Canvas>();
        _image = GetComponentInChildren<Image>();
        _originalColour = _image.color;
    }
    
    public void Init(CardType cardType, SpellConfig config, CardPopupItem cardPopupItem, CardPopup popup, UISettings uiSettings)
    {
        _cardType = cardType;
        _cardPopupItem = cardPopupItem;
        _cardPopupItem.BeginDrag += OnBeginDrag;
        _cardPopupItem.EndDrag += OnEndDrag;
        _cardPopupItem.PointerEnter += OnPointerEnter;
        _cardPopupItem.PointerExit += OnPointerExit;
        cardPopup = popup;
        settings = uiSettings;
        InitConfig(config);
    }

    protected virtual void InitConfig(SpellConfig config)
    {
        _oneTimeUse = config.oneTimeUse;
        damage = config.damage;
        type = config.type;
    }

    private void Update()
    {
        if (!_cardPopupItem) return;
        HandPositioning();
        Follow();
        Rotate();

        if (_cardPopupItem.State == CardState.Hovering || _cardPopupItem.State == CardState.Dragging)
        {
            OnInteracting();
        }
    }

    private void HandPositioning()
    {
        float normalizedPosition = _cardPopupItem.GetNormalizedPosition();
        _curveYOffset = (settings.positionCurve.Evaluate(normalizedPosition) *
                         settings.positionInfluence);
        _curveRotationOffset = settings.rotationCurve.Evaluate(normalizedPosition) * settings.rotationInfluence;
    }

    private void Follow()
    {
        Vector3 verticalOffset = Vector2.up * (_cardPopupItem.State == CardState.Dragging ? 0 : _curveYOffset);
        _rect.position = Vector3.Lerp(_rect.position, _cardPopupItem.Rect.position + verticalOffset, settings.followSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 movementVector = _cardPopupItem.Rect.InverseTransformPoint(_rect.position);
        _movementDelta = Vector3.Lerp(_movementDelta, movementVector, 25 * Time.deltaTime);
        Vector3 movementRotation = (_cardPopupItem.State == CardState.Dragging ? _movementDelta : movementVector) * settings.rotationAmount;
        _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, settings.rotationSpeed * Time.deltaTime);
        float rotationOffset = _cardPopupItem.State == CardState.Dragging ? 0f : _curveRotationOffset;
        _rect.eulerAngles = new Vector3(_rect.eulerAngles.x, _rect.eulerAngles.y, Mathf.Clamp(_rotationDelta.x, -settings.maxRotation, settings.maxRotation) + rotationOffset);
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

    public void UpdateIndex()
    {
        transform.SetSiblingIndex(_cardPopupItem.transform.parent.GetSiblingIndex());
    }

    private void OnBeginDrag(CardPopupItem cardPopupItem)
    {
        cardPopup.TooltipPopup.Hide();
        cardPopup.TooltipPopup.Disable();
        OnStartDragging();
    }
    
    private void OnEndDrag(CardPopupItem cardPopupItem)
    {
        cardPopup.TooltipPopup.Enable();
        if (CanApply())
        {
            ApplySpell();
        }
        else
        {
            _image.color = _originalColour;
        }
        OnStopInteracting();
    }

    protected abstract bool CanApply();

    protected virtual void ApplySpell()
    {
        List<Enemy> targets = GetTargets();
        for (int i = 0; i < targets.Count; ++i)
            Apply(targets[i]);
        Use();
    }

    protected virtual void Use()
    {
        DisableOutline();
        ServiceLocator.Instance.Get<IGameManager>().GetGame().UseCard(_cardType);
        if (_oneTimeUse)
        {
            Debug.Log("Use up card");
            cardPopup.RemoveCard(_cardPopupItem);
            RemoveAsync().Forget();
        }
    }

    private void DisableOutline()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetGame().Enemies;
        for (int i = 0; i < enemies.Count; ++i)
            enemies[i].DisableOutline();
    }
    

    private async UniTask RemoveAsync()
    {
        // Fade out the image over 0.5 seconds
        _image.DOFade(0f, settings.cardFadeTime);
        await UniTask.WaitForSeconds(settings.cardFadeTime);
        Destroy(gameObject);
    }

    private void OnPointerEnter(CardPopupItem cardPopupItem)
    {
        OnStartInteracting();
    }
    
    private void OnPointerExit(CardPopupItem cardPopupItem)
    {
        OnStopInteracting();
    }

    protected virtual void OnStartDragging()
    {
        Color baseColour = _image.color;
        baseColour.a = settings.dragAlpha;
        _image.color = baseColour;
    }

    protected virtual void OnStartInteracting()
    {
        cardPopup.TooltipPopup.Init(_cardType);
        interactingTime = 0f;
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(Vector3.one * settings.hoverScale, settings.hoverScaleDuration).SetEase(Ease.OutBack);
        _shakeTween?.Kill();
        _shakeTween = transform.GetChild(0).DOPunchRotation(Vector3.forward * (settings.hoverPunchAngle/2), settings.hoverScaleDuration, 20, 1).SetId(2);
    }

    protected virtual void OnInteracting()
    {
        interactingTime += Time.deltaTime;
    }

    protected virtual void OnStopInteracting()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(Vector3.one, settings.hoverScaleDuration).SetEase(Ease.InBack);
        // _canvas.sortingOrder = _cardPopupItem.GetParentIndex();
        cardPopup.TooltipPopup.Hide();
        interactingTime = 0f;
        DisableOutline();
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
