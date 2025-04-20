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

        _image.color = CanActivate ? _uiSettings.activateEnabled : _uiSettings.activateDisabled;
    }
}
