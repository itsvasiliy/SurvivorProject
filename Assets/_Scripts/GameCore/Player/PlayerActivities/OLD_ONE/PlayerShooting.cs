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

    private Transform target;

    private float targetHeight;

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

            if (playerStateController.GetState() == PlayerStates.Idle ||
                playerStateController.GetState() == PlayerStates.Shooting)
            {
                Transform closestEnemy = GetClosestEnemy();

                if (closestEnemy != null)
                {
                    StartCoroutine(ShootTheTarget(closestEnemy));
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
                            targetHeight = collider.bounds.size.y;
                        }
                    }
                }
            }
        }

        return closestTarget;
    }


    private IEnumerator ShootTheTarget(Transform _target)
    {
        RotatePlayerToTheTarget(_target.position);

        MakeRpcCallsServerRpc();


        hideBowCoroutine = StartCoroutine(HideBowIEnumerator(3f));

        animator.SetTrigger("IsShooting");

        yield return new WaitForSeconds(shootAnArrowDelay);
        ShootAnArrowServerRpc(_target.position);
    }

    private IEnumerator HideBowIEnumerator(float delay)
    {
        yield return new WaitForSeconds(delay);
        bowGameobject.SetActive(false);
    }


    [ServerRpc(RequireOwnership = false)]
    private void ShootAnArrowServerRpc(Vector3 position) => ShootAnArrowClientRpc(position);


    [ClientRpc]
    private void ShootAnArrowClientRpc(Vector3 targetPosition)
    {
        if (playerStateController.GetState() != PlayerStates.Shooting)
            return; //player aborted attack by exiting the shooting state

        GameObject arrow = Instantiate(ammoPrefab, muzzleOfShot.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetResourceController(playerResourceController); //set player's resource controller for get drop resource from enemy

        targetPosition.y = targetHeight / 3;
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


    [ServerRpc(RequireOwnership = false)]
    private void MakeRpcCallsServerRpc() => MakeRpcCallsClientRpc();


    [ClientRpc]
    private void MakeRpcCallsClientRpc()
    {
        playerStateController.SetState(PlayerStates.Shooting);
        bowGameobject.SetActive(true);
        if (hideBowCoroutine != null)
        {
            StopCoroutine(hideBowCoroutine);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}