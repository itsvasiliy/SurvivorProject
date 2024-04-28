using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Following : EnemyMovement
{
    [SerializeField] float stopFollowingAt;

    private NavMeshAgent _navMeshAgent;

    private Animator animator;

    private Transform detectedPlayer;


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
            if (detectedPlayer != null)
                if (base.IsPlayerInDetectionRadius(detectedPlayer) == false)
                    ResetDestination();

            if (IsCanMove() && enabled && IsDistanceReached() == false)
            {
                if (detectedPlayer == null)
                    detectedPlayer = GetClosestPlayer();

                if (detectedPlayer != null)
                {
                    if (detectedPlayer.gameObject.GetComponent<PlayerHealthController>().enabled)
                    {
                        animator.SetBool("IsWalking", true);
                        _navMeshAgent.SetDestination(detectedPlayer.position);
                    }
                    else
                        ResetDestination();
                }
            }
            else
                ResetDestination();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (detectedPlayer != null)
            {
                float distance = Vector3.Distance(detectedPlayer.position, base.enemyTransform.position);

                if (distance > base.detectionRadius || distance <= stopFollowingAt)
                    StopFollowing();
            }
        }
    }

    private float GetDistanceToTarget() =>
        Vector3.Distance(detectedPlayer.position, base.enemyTransform.position);

    private bool IsDistanceReached()
    {
        if (detectedPlayer != null)
        {
            if (GetDistanceToTarget() < stopFollowingAt)
                return true;
            else return false;
        }
        return false;
    }


    private void ResetDestination()
    {
        detectedPlayer = null;
        StopFollowing();
    }

    private void StopFollowing()
    {
        animator.SetBool("IsWalking", false);
        _navMeshAgent.SetDestination(transform.position);
    }
}
