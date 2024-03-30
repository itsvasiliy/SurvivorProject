using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] protected Transform enemyTransform;

    [SerializeField] protected float detectionRadius;

    protected Transform GetClosestPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, detectionRadius);

        float closestDistance = float.MaxValue;
        Collider closestCollider = null;

        foreach (Collider collider in colliders)
        {
            PlayerStateController aimTarget = collider.GetComponent<PlayerStateController>();

            if (aimTarget != null)
            {
                float distance = Vector3.Distance(enemyTransform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }

        if (closestCollider != null)
        {
            return closestCollider.transform;
        }

        return null;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(enemyTransform.position, detectionRadius);
    }
#endif

}
