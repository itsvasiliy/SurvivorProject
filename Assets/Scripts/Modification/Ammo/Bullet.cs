using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        while (Vector3.Distance(_transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - _transform.position).normalized;
            float distanceToMove = speed * Time.fixedDeltaTime;

            _transform.position += direction * distanceToMove;

            yield return null;
        }

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
    }
}