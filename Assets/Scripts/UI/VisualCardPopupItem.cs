using UnityEngine;
using UnityEngine.Serialization;

public class VisualCardPopupItem : MonoBehaviour
{
    [Header("Follow Parameters")] 
    [SerializeField] private float followSpeed = 5f;

    [Header("Rotation Parameters")] 
    [SerializeField] private float rotationAmount = 20f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float maxRotation = 60f;
    [SerializeField] private float tiltSpeed = 20f;

    private CardPopupItem _cardPopupItem;
    private Vector3 _movementDelta;
    private Vector3 _rotationDelta;

    public void Init(CardPopupItem cardPopupItem)
    {
        _cardPopupItem = cardPopupItem;
    }

    private void Update()
    {
        if (!_cardPopupItem) return;
        Follow();
        Rotate();
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
}
