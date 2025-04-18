using UnityEngine;

public class VisualCard : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void MoveTo(Vector2 position)
    {
        _rectTransform.position = position;
    }
}
