using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private int damage;

    [SerializeField] protected float speed;
    [SerializeField] protected float lifeTime;

    public Vector3 targetPosition;

    protected bool isExploded = false;
    protected bool isPooled = false;

    protected Action onBulletCollision;

    private ResourceController playerResourceController;

    private Coroutine moveCoroutine;

    public void SetPlayerResourceController(ResourceController value) => playerResourceController = value;

    public virtual void Launch(Vector3 startPosition, Vector3 target, Action _onBulletCollision)
    {
        LaunchInit(startPosition, target, _onBulletCollision);

        RotateToTarget(targetPosition);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTarget());

        Invoke(nameof(ReturnToPool), lifeTime);
    }

    protected void LaunchInit(Vector3 startPosition, Vector3 target, Action _onBulletCollision)
    {
        isPooled = false;
        transform.position = startPosition;
        targetPosition = target;
        onBulletCollision = _onBulletCollision;
    }

    protected void RotateToTarget(Vector3 targetPosition)
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

    protected void ReturnToPool()
    {
        if (isPooled)
            return;
        isPooled = true;

        CancelInvoke();

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = null;
        targetPosition = Vector3.zero;
        isExploded = false;
        playerResourceController = null;

        onBulletCollision?.Invoke();
    }

    protected void Explode(GameObject _go)
    {
        if (isExploded)
            return;

        isExploded = true;
        Debug.Log($"damaging {_go.name}");

        if (_go.TryGetComponent<IDamageable>(out IDamageable _aimTarget))
            _aimTarget.GetDamage(damage, playerResourceController);

        ReturnToPool();
    }
}