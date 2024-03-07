using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Transform _transform;

    [SerializeField] private float damage;
    [SerializeField] private float damageRadius;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private List<IAimTarget> aimTargets = new List<IAimTarget>();

    private Vector3 targetPosition;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position, 20f);

        float closestDistance = float.MaxValue;
        Collider closestCollider = null;

        foreach (Collider collider in colliders)
        {
            IAimTarget aimTarget = collider.GetComponent<IAimTarget>();

            if (aimTarget != null)
            {
                float distance = Vector3.Distance(_transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }

        if (closestCollider != null)
        {
            targetPosition = closestCollider.transform.position;
            StartCoroutine(MoveToTarget());
        }

        Invoke(nameof(Explode), lifeTime);
    }

    private IEnumerator MoveToTarget()
    {
        while (true)
        {
            Vector3 direction = (targetPosition - _transform.position).normalized;
            float distanceToMove = speed * Time.fixedDeltaTime;

            _transform.position += direction * distanceToMove;

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position, damageRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
            {
                _aimTarget.GetDamage();
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