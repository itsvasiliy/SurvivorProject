using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Spider : EnemyMovement
{
    [Header("Spyder")]

    [SerializeField] private Animator _animator;

    [SerializeField] private AnimationClip accelerationClip;

    [SerializeField] private float accelerationSpeed;

    private NavMeshAgent _navMeshAgent;

    private float accelerationDuration;
    private float checkForPlayerRate = 0.4f;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.avoidancePriority = Random.Range(1, 99);

        accelerationDuration = accelerationClip.length;

        StartCoroutine(CheckForPlayer());
    }


    private void OnDisable() => StopAllCoroutines();


    private IEnumerator CheckForPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkForPlayerRate);

            Transform closestPlayer = base.GetClosestPlayer();

            if(closestPlayer != null)
            {
                if (Vector3.Distance(base.enemyTransform.position, closestPlayer.position) < (base.detectionRadius / 1.7f))
                {
                    StartCoroutine(AccelerateTowardsPlayer(closestPlayer, accelerationDuration));
                    yield return new WaitForSeconds(accelerationDuration);
                }
                else
                {
                    _animator.SetBool("Following", true);
                    _navMeshAgent.SetDestination(closestPlayer.position);
                }
            }

            if(_navMeshAgent.remainingDistance < 0.1f)
            {
                _animator.SetBool("Following", false);
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
            _animator.SetBool("Acceleration", true);

            _navMeshAgent.SetDestination(playerTransform.position);
            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        _navMeshAgent.speed = regularSpeed;
        _animator.SetBool("Acceleration", false);
    }
}
