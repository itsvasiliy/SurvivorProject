using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Spyder : EnemyMovement
{
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float accelerationDuration;

    private NavMeshAgent _navMeshAgent;

    private float checkForPlayerRate = 0.4f;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(CheckForPlayer());
    }

    private IEnumerator CheckForPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkForPlayerRate);

            Transform closestPlayer = base.GetClosestPlayer();

            if(closestPlayer != null)
            {
                if (Vector3.Distance(base.enemyTransform.position, closestPlayer.position) < (base.detectionRadius / 2f))
                {
                    StartCoroutine(AccelerateTowardsPlayer(closestPlayer, accelerationDuration));
                    yield return new WaitForSeconds(accelerationDuration);
                }
                else
                {
                    _navMeshAgent.SetDestination(closestPlayer.position);
                }
            }
        }
    }

    private IEnumerator AccelerateTowardsPlayer(Transform playerTransform, float duration)
    {
        float timer = 0f;
        float regularSpeed = _navMeshAgent.speed;

        _navMeshAgent.speed = accelerationSpeed;

        while (timer < duration)
        {
            _navMeshAgent.SetDestination(playerTransform.position);

            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }

        _navMeshAgent.speed = regularSpeed;
    }
}
