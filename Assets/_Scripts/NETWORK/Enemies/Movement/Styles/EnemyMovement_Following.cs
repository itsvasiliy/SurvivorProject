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
    private Coroutine searchForPlayerCoroutine;


    private void OnEnable()
    {
        if (IsSpawned == false)
            return;
        searchForPlayerCoroutine = StartCoroutine(SearchForPlayer());
        StartCoroutine(CheckPlayerDistance());
    }

    private void OnDisable()
    {
        if(IsSpawned == false) 
            return;

        if (searchForPlayerCoroutine != null)
            StopAllCoroutines();
        searchForPlayerCoroutine = null;
        ResetDestination();

    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        searchForPlayerCoroutine = StartCoroutine(SearchForPlayer());
        StartCoroutine(CheckPlayerDistance());
    }


    private IEnumerator SearchForPlayer()
    {
        while (true)
        {
            detectedTarget = GetClosestTarget();

            if (IsCanMove() && enabled && IsDistanceReached() == false)
            {
                if (detectedTarget != null)
                {
                    if (detectedTarget.gameObject.GetComponent<IHealthController>().IsAlive())
                    {
                        animator.SetBool("IsWalking", true);
                        _navMeshAgent.SetDestination(detectedTarget.position);
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
        detectedTarget = null;
        StopFollowing();
    }

    private void StopFollowing()
    {
        animator.SetBool("IsWalking", false);
        _navMeshAgent.SetDestination(transform.position);
    }
}
