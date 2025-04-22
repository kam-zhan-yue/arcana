using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.UI;

public class ActivationZone : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _image;
    public bool CanActivate { get; private set; }

    private Camera _uiCamera;
    private UISettings _uiSettings;
    public bool _restrict = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _uiCamera = Camera.main;
    }

    private void Start()
    {
        _uiSettings = ServiceLocator.Instance.Get<IGameManager>().GetGame().Database.uiSettings;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        bool isNowOver = RectTransformUtility.RectangleContainsScreenPoint(
            _rectTransform, mousePosition, _uiCamera);

        if (isNowOver && !CanActivate)
        {
            CanActivate = true;
        }
        else if (!isNowOver && CanActivate)
        {
            CanActivate = false;
        }

        if (CanActivate && !_restrict)
        {
            _image.color = _uiSettings.activateEnabled;
        }
        else if (CanActivate && _restrict)
        {
            _image.color = _uiSettings.activateRestricted;
        }
        else
        {
            _image.color = _uiSettings.activateDisabled;
        }
    }

    public void SetRestricted(bool restricted)
    {
        _restrict = restricted;
    }
}
