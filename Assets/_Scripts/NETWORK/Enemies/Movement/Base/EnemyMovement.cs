using Unity.Netcode;
using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] protected Transform enemyTransform;

    [SerializeField] protected float detectionRadius;

    protected bool canMove = true;


    public bool IsCanMove() => canMove;

    public void SetCanMoveStatus(bool status) => canMove = status;


    protected Transform GetClosestPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, detectionRadius);

        float closestDistance = float.MaxValue;
        Collider closestCollider = null;

        foreach (Collider collider in colliders)
        {
            var aimTarget = collider.GetComponent<PlayerHealthController>();

            if (aimTarget != null && aimTarget.enabled)
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


    protected bool IsPlayerInDetectionRadius(Transform detectedPlayer)
    {
        var distanceToPlayer = Vector3.Distance(detectedPlayer.position, enemyTransform.position);

        if (distanceToPlayer < detectionRadius)
            return true;
        return false;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyTransform.position, detectionRadius);
    }
#endif

}