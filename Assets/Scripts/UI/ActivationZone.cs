using UnityEngine;
using UnityEngine.UI;

public class ActivationZone : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _image;
    private Material _materialInstance;
    private static readonly int Activate = Shader.PropertyToID("_Can_Activate");
    public bool CanActivate { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _materialInstance = Instantiate(_image.material);
        _image.material = _materialInstance;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        bool isNowOver = RectTransformUtility.RectangleContainsScreenPoint(
            _rectTransform, mousePosition, null);

        if (isNowOver && !CanActivate)
        {
            CanActivate = true;
        }
        else if (!isNowOver && CanActivate)
        {
            CanActivate = false;
        }
        _materialInstance.SetFloat(Activate, CanActivate ? 1f : 0f);
    }
}
