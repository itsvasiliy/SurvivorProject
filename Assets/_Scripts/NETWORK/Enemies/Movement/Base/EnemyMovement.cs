using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public enum TargetPriority
{
    Players,
    Structures,
    Mixed,
}

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] public TargetPriority targetPriority;

    [SerializeField] protected Transform enemyTransform;

    [SerializeField] protected float detectionRadius;

    [SerializeField] protected GameObject footstepsSound;


    protected bool canMove = true;
    private bool isCurrentTargetStructure = false;
    [HideInInspector] public bool isPlayerNear;


    public bool IsCanMove() => canMove;
    public void SetCanMoveStatus(bool status) => canMove = status;

    protected Transform GetClosestTarget()
    {
        switch (targetPriority)
        {
            case TargetPriority.Players:
                return GetClosestPlayer();

            case TargetPriority.Structures:
                return GetClosestStructure();

            case TargetPriority.Mixed:
                return GetClosestAnyTarget();

            default: return GetClosestPlayer();
        }
    }

    private Transform GetClosestPlayer() => GetTarget(isPlayer: true, isStructure: false);
    private Transform GetClosestStructure() => GetTarget(isPlayer: false, isStructure: true);
    private Transform GetClosestAnyTarget() => GetTarget(isPlayer: true, isStructure: true);


    private Transform GetTarget(bool isStructure, bool isPlayer)
    {
        isPlayerNear = false;
        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, detectionRadius);

        Collider closestCollider = null;
        IHealthController healthController;

        foreach (Collider collider in colliders)
        {
            healthController = GetHealthController(collider, isPlayer, isStructure);

            if (healthController != null && healthController.IsAlive())
            {
                //if current detected target is not player, skip if closestTarget already found
                if (closestCollider != null && (isCurrentTargetStructure && (isPlayer || isPlayerNear)))
                    continue;

                closestCollider = collider;
                if (isPlayerNear)
                    break;
            }
        }

        if (closestCollider != null)
            return closestCollider.transform;

        return null;
    }


    private IHealthController GetHealthController(Collider collider, bool isPlayer, bool isStructure)
    {
        IHealthController healthController;
        if (isPlayer && isStructure)
        {
            healthController = GetPlayerHealth(collider);
            if (healthController != null)
                return healthController;

            return GetStructureHealth(collider);
        }

        else if (isPlayer)
            return GetPlayerHealth(collider);

        else if (isStructure)
            return GetStructureHealth(collider);

        return null;
    }

    private IHealthController GetStructureHealth(Collider collider)
    {
        var healthController = collider.GetComponent<StructureHealthController>();
        if (healthController != null)
        {
            isCurrentTargetStructure = true;
            return healthController;
        }
        return null;
    }

    private IHealthController GetPlayerHealth(Collider collider)
    {
        var healthController = collider.GetComponent<PlayerHealthHandlerForController>();
        if (healthController != null)
        {
            isCurrentTargetStructure = false;
            isPlayerNear = true;
            return healthController;
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