using Kuroneko.UtilityDelivery;
using UnityEngine;

public enum MovementStatus
{
    None,
    Knockback,
}

public class Enemy : MonoBehaviour, ISpellTarget, IFreezeTarget
{
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody _rigidbody;
    private Status _status = Status.None;
    private MovementStatus _movementStatus = MovementStatus.None;
    private float _frozenTimer = 0.0f;
    private float _knockbackTimer = 0.0f;

    public Status Status => _status;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public Transform GetTransform()
    {
        return transform;
    }

    private void Update()
    {

        if (_status == Status.Frozen)
        {
            _frozenTimer -= Time.deltaTime;
            if (_frozenTimer <= 0f)
                Unfreeze();
            return;
        }

        switch (_movementStatus)
        {
            case MovementStatus.None:
                Player player = ServiceLocator.Instance.Get<IGameManager>().GetPlayer();
                Vector3 nextPosition =
                    Vector3.MoveTowards(_rigidbody.position, player.transform.position, moveSpeed * Time.deltaTime);
                _rigidbody.MovePosition(nextPosition);
                break;
            case MovementStatus.Knockback:
                break;
        }
    }

    public void Freeze(float frozenTime)
    {
        _frozenTimer = frozenTime;
        _status = Status.Frozen;
        _rigidbody.isKinematic = true;
    }

    public void Unfreeze()
    {
        _status = Status.None;
        _rigidbody.isKinematic = false;
    }

    public void Knockback(Vector3 knockbackForce, float knockbackTime)
    {
        _movementStatus = MovementStatus.Knockback;
        _rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
        _knockbackTimer = knockbackTime;
    }
}
