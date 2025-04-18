using UnityEngine;
using UnityEngine.Serialization;

public class VisualCard : MonoBehaviour
{
    [Header("Follow Parameters")] 
    [SerializeField] private float followSpeed = 5f;

    [Header("Rotation Parameters")] 
    [SerializeField] private float rotationAmount = 20f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float maxRotation = 60f;
    [SerializeField] private float tiltSpeed = 20f;

    private Card _card;
    private Vector3 _movementDelta;
    private Vector3 _rotationDelta;

    public void Init(Card card)
    {
        _card = card;
    }

    private void Update()
    {
        if (!_card) return;
        Follow();
        Rotate();
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, _card.transform.position, followSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 movement = (transform.position - _card.transform.position);
        _movementDelta = Vector3.Lerp(_movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (_card.State == CardState.Dragging ? _movementDelta : movement) * rotationAmount;
        _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(_rotationDelta.x, -maxRotation, maxRotation));
    }
}
