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

    private Vector3 targetPosition;

    public Vector3 SetTargetPOsition
    {
        set
        {
            targetPosition = value;
            StartCoroutine(MoveToTarget());
        }
    }

    private void Start()
    {
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