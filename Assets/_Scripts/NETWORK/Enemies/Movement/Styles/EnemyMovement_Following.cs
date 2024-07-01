using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Following : EnemyMovement
{
    [SerializeField] float stopFollowingAt;

    private NavMeshAgent _navMeshAgent;

    private Animator animator;

    private Transform detectedTarget;

    private bool isFollowing = false;


    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(SearchForPlayer());
        StartCoroutine(CheckPlayerDistance());
    }

    private IEnumerator SearchForPlayer()
    {
        while (true)
        {
            detectedTarget = GetClosestTarget();

            if (detectedTarget == null || base.IsPlayerInDetectionRadius(detectedTarget) == false)
                if(isFollowing)
                    ResetDestination();


            if (IsCanMove() && enabled && IsDistanceReached() == false)
            {
                if (detectedTarget != null)
                {
                    if (detectedTarget.gameObject.GetComponent<IHealthController>().IsAlive())
                        SetFollowPosition(detectedTarget.position);
                    else
                        ResetDestination();
                }
            }
            else
                ResetDestination();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SetFollowPosition(Vector3 position)
    {
        Debug.Log("Moving to player");

        animator.SetBool("IsWalking", true);
        _navMeshAgent.SetDestination(position);
        isFollowing = true;
        base.footstepsSound.SetActive(isFollowing);
    }

    private IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (detectedTarget != null)
            {
                float distance = Vector3.Distance(detectedTarget.position, base.enemyTransform.position);

                if (distance > base.detectionRadius || distance <= stopFollowingAt)
                    detectedTarget = null;
            }
        }
    }

    private float GetDistanceToTarget() =>
        Vector3.Distance(detectedTarget.position, base.enemyTransform.position);

    private bool IsDistanceReached()
    {
        if (detectedTarget != null)
        {
            if (GetDistanceToTarget() < stopFollowingAt)
                return true;
            else return false;
        }
        return false;
    }


    private void ResetDestination()
    {
        Debug.Log("Destination reseted");
        isFollowing = false;
        detectedTarget = null;
        StopFollowing();
    }

    private void StopFollowing()
    {
        Debug.Log("Following is stoped");

        animator.SetBool("IsWalking", false);
        _navMeshAgent.SetDestination(transform.position);
        base.footstepsSound.SetActive(isFollowing);
    }
}
