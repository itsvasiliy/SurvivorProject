using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Following : EnemyMovement
{
    private NavMeshAgent _navMeshAgent;

    private Transform detectedPlayer;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        detectedPlayer = GetClosestPlayer();

        StartCoroutine(SearchForPlayer());
        StartCoroutine(CheckPlayerDistance());
    }

    private IEnumerator SearchForPlayer()
    {
        while (true)
        {
            if (detectedPlayer == null)
            {
                detectedPlayer = GetClosestPlayer();

                if(detectedPlayer != null)
                {
                    _navMeshAgent.SetDestination(detectedPlayer.position);
                }
            }
            else
            {
                _navMeshAgent.SetDestination(detectedPlayer.position);
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
                {
                    detectedPlayer = null;
                }
            }
        }
    }
}
