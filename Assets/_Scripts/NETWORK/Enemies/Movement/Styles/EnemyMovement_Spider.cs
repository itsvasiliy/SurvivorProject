using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement_Spider : EnemyMovement
{
    [Header("Spider")]

    [SerializeField] private Animator _animator;

    [SerializeField] private AnimationClip accelerationClip;

    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float distanceToStartAcceleration;

    [SerializeField] private EnemyHealthController healthController;

    [SerializeField] private GameObject healthSlider;

    private NavMeshAgent _navMeshAgent;

    private float accelerationDuration;
    private float checkForPlayerRate = 0.4f;


    private void OnEnable()
    {
        if (IsSpawned)
        {
            SetStealthModeStatus(true); //turn on stealth mode
            StartCoroutine(CheckForPlayer());
        }
    }
    private void OnDisable() => StopAllCoroutines();


    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.avoidancePriority = Random.Range(1, 99);

        accelerationDuration = accelerationClip.length;

        SetStealthModeStatus(true); //turn on stealth mode

        StartCoroutine(CheckForPlayer());
    }


    private IEnumerator CheckForPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkForPlayerRate);

            Transform closestPlayer = base.GetClosestTarget();

            if (closestPlayer != null)
            {
                if (Vector3.Distance(base.enemyTransform.position, closestPlayer.position) < distanceToStartAcceleration)
                {
                    StartCoroutine(AccelerateTowardsPlayer(closestPlayer, accelerationDuration));
                    SetStealthModeStatus(false); //turn off stealth mode
                    yield return new WaitForSeconds(accelerationDuration);
                }
                else
                {
                    SetStealthModeStatus(true); //turn on stealth mode
                    _animator.SetBool("Following", true);
                    _navMeshAgent.SetDestination(closestPlayer.position);
                }
            }

            if (_navMeshAgent.remainingDistance < 0.1f || _navMeshAgent.velocity == Vector3.zero)
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


    private void SetStealthModeStatus(bool status)
    {
        healthSlider.SetActive(!status);
        healthController.SetAttackableStatus(!status);
    }
}
