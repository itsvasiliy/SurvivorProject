using UnityEngine;
using UnityEngine.AI;

public class EnemyBot : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float wanderingRadius = 5f;
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject playerWeaponPrefab;

    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isDead)
        {
            SearchForPlayer();
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= detectionRadius)
                {
                    agent.SetDestination(player.transform.position);
                    animator.SetBool("Idle", false); // Установить Idle в false, когда враг идет к игроку
                }
                else
                {
                    Wander();
                    animator.SetBool("Idle", true); // Установить Idle в true, когда враг бродит
                }
            }
            else
            {
                Wander();
                animator.SetBool("Idle", true); // Установить Idle в true, если игрок не обнаружен
            }
        }
    }

    void SearchForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                player = collider.gameObject;
                return;
            }
        }
        player = null;
    }

    void Wander()
    {
        if (!agent.hasPath || agent.remainingDistance < 1f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderingRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderingRadius, 1);
            Vector3 finalPosition = hit.position;

            agent.SetDestination(finalPosition);
            animator.SetBool("Idle", true); // Установить Idle в true, когда враг бродит
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("Die", true); // Установить Die в true при смерти врага
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDead && collision.gameObject == playerWeaponPrefab)
        {
            TakeDamage(10);
        }
    }
}
