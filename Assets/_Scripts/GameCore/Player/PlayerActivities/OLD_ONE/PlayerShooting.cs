using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [Header("Player's shoot properties")]
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject bowGameobject;

    [SerializeField] private Transform muzzleOfShot;

    [SerializeField] private float shootingRadius;
    [SerializeField] private float shootingRate;
    [SerializeField] private float shootAnArrowDelay;
    [SerializeField] private float checkForEnemyRate;
    [SerializeField] private float arrowSpeed;

    [Header("Player")]
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Animator animator;

    [SerializeField] private PlayerStateController playerStateController;


    [SerializeField] private ResourceController playerResourceController;

    private Coroutine hideBowCoroutine;

    private Vector3 targetPosition;

    private void Start()
    {
        if (IsOwner)
        {
            StartCoroutine(EnemyDetecting());
        }
    }

    private IEnumerator EnemyDetecting()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkForEnemyRate);

            if (playerStateController.GetState() == PlayerStates.Idle)
            {
                Transform closestEnemy = GetClosestEnemy();

                if (closestEnemy != null)
                {
                    ShootTheTargetServerRPC(closestEnemy.position);
                    yield return new WaitForSeconds(shootingRate);
                }
            }
        }
    }

    private Transform GetClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, shootingRadius);

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget aimTarget))
            {
                if (aimTarget.IsAlive() && aimTarget.IsVisible())
                {
                    float distance = Vector3.Distance(playerTransform.position, collider.transform.position);
                    Vector3 directionToTarget = collider.transform.position - playerTransform.position;

                    if (IsTargetBehindObstacle(collider.transform, directionToTarget, distance) == false)
                    {
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestTarget = collider.transform;
                        }
                    }
                }
            }
        }

        return closestTarget;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootTheTargetServerRPC(Vector3 targetPosition)
    {
        ShootTheTargetClientRpc(targetPosition);
    }

    [ClientRpc]
    private void ShootTheTargetClientRpc(Vector3 targetPosition)
    {
        bowGameobject.SetActive(true);

        if(hideBowCoroutine != null)
        {
            StopCoroutine(hideBowCoroutine);
        }

        hideBowCoroutine = StartCoroutine(HideBowIEnumerator(3f));

        RotatePlayerToTheTarget(targetPosition);

        animator.SetTrigger("IsShooting");
        this.targetPosition = targetPosition;

        Invoke(nameof(ShootAnArrow), shootAnArrowDelay);
    }

    private IEnumerator HideBowIEnumerator(float delay)
    {
        yield return new WaitForSeconds(delay);
        bowGameobject.SetActive(false);
    }

    private void ShootAnArrow()
    {
        RotatePlayerToTheTarget(targetPosition);

        GameObject arrow = Instantiate(ammoPrefab, muzzleOfShot.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetResourceController(playerResourceController); //set player's resource controller for get drop resource from enemy
        Vector3 direction = (targetPosition - muzzleOfShot.position).normalized;
        arrow.transform.rotation = Quaternion.LookRotation(direction);

        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        arrowRigidbody.AddForce(direction * arrowSpeed, ForceMode.Impulse);

        Destroy(arrow, 1.5f);
    }

    #region AutoRotate
    private void RotatePlayerToTheTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - playerTransform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, 1f);
    }
    #endregion

    private bool IsTargetBehindObstacle(Transform targetTransform, Vector3 directionToTarget, float distance)
    {
        // Check if there's an obstacle between the player and the target
        var playerpos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.2f, playerTransform.position.z);
        if (Physics.Raycast(playerpos, directionToTarget, out RaycastHit hit, distance))
        {
            // Ensure that the hit object is not the target itself
            if (hit.transform != targetTransform)
            {
                Debug.Log($"obstacle to shoot is {hit.transform.name}");
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}