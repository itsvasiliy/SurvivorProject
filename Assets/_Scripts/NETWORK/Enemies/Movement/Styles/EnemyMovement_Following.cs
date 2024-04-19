using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Following : EnemyMovement
{
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

                if (distance > base.detectionRadius)
                    ResetDestination();
            }
        }
    }

    private void ResetDestination()
    {
        animator.SetBool("IsWalking", false);
        detectedPlayer = null;
        _navMeshAgent.SetDestination(transform.position);
    }
}
