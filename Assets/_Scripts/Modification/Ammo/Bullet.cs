using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private int damage;

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    [HideInInspector] public Vector3 targetPosition;

    private bool isExploded = false;

    [SerializeField] private ResourceController playerResourceController;

    private void Start()
    {
        if (targetPosition == Vector3.zero)
            Debug.LogError($"Set target position first before spawn bullet.\n Error caused by {name}");

        RotateToTarget(targetPosition);
        StartCoroutine(MoveToTarget());

        Invoke(nameof(DestroyThis), lifeTime);
    }

    public void SetTarget(Vector3 vector3) => targetPosition = vector3;
    public void SetPlayerResourceController(ResourceController value) => playerResourceController = value;


    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x + transform.rotation.eulerAngles.x,
            targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 direction = (targetPosition - _transform.position).normalized;
        while (true)
        {
            float distanceToMove = speed * Time.fixedDeltaTime;

            _transform.position += direction * distanceToMove;

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision) => Explode(collision.gameObject);

    private void DestroyThis() => Destroy(gameObject);

    private void Explode(GameObject _go)
    {
        if (isExploded)
            return;

        isExploded = true;

        if (_go.TryGetComponent<IDamageable>(out IDamageable _aimTarget))
            _aimTarget.GetDamage(damage, playerResourceController);

        Destroy(gameObject);
    }
}