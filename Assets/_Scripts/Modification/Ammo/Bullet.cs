using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private int damage;
    [SerializeField] private float damageRadius;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private Vector3 targetPosition;

    private bool isExploded = false;

    private void Start()
    {
        if (targetPosition == Vector3.zero)
            Debug.LogError($"Set target position first before spawn bullet.\n Error caused by {name}");

        StartCoroutine(MoveToTarget());
        Invoke(nameof(Explode), lifeTime);
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        RotateToTarget(target);
    }

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition);
        transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x + transform.rotation.eulerAngles.x,
            targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
    }

    private IEnumerator MoveToTarget()
    {
        while (!isExploded)
        {
            Vector3 direction = (targetPosition - _transform.position).normalized;
            float distanceToMove = speed * Time.fixedDeltaTime;

            _transform.position += direction * distanceToMove;

            // Check if the bullet is close enough to the target to explode
            float distanceToTarget = Vector3.Distance(_transform.position, targetPosition);
            if (distanceToTarget <= damageRadius)
            {
                Explode();
                yield break; // Exit the coroutine
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploded)
            return;

        isExploded = true;

        Collider[] colliders = Physics.OverlapSphere(_transform.position, damageRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable _aimTarget))
            {
                _aimTarget.GetDamage(damage);
            }
        }

        if (IsOwner)
            DespawnServerRpc();
    }

    [ServerRpc]
    private void DespawnServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}
