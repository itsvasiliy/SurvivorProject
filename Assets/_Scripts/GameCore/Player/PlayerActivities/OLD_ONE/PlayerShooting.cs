using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Player's shoot properties")]
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject weaponGameobject;

    [SerializeField] private Transform muzzleOfShot;

    [SerializeField] private float shootingRadius;
    [SerializeField] private float attacksPerSecond;


    [Header("Player")]
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Animator animator;

    [SerializeField] private PlayerStateController playerStateController;

    [SerializeField] private ResourceController playerResourceController;

    private ObjectPool<Bullet> bulletPool;


    private void Awake()
    {
        int bulletPoolAmount = 3;
        bulletPool = new ObjectPool<Bullet>(Preload, GetAction, ReturnAction, bulletPoolAmount);
    }

    private Transform closestTarget;

    private bool isShooting = false;

    private float targetHeight;

    private float fireRate;


    private void Start()
    {
        animator.SetFloat("AttackSpeed", attacksPerSecond);
        fireRate = 1 / attacksPerSecond;
    }


    private void Update()
    {
        var playerState = playerStateController.GetState();
        if (playerState != PlayerStates.Idle && playerState != PlayerStates.Shooting)
            return;

        if (isShooting)
        {
            RotateToTarget(closestTarget.position);
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, shootingRadius);

        closestTarget = null;
        float distanceToClosestTarget = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
            {
                if (_aimTarget.IsAlive())
                {
                    Vector3 directionToTarget = collider.transform.position - playerTransform.position;
                    float distance = directionToTarget.magnitude;

                    // Check if there's an obstacle between the player and the target
                    if (IsTargetBehindObstacle(collider.transform, directionToTarget, distance) == false)
                    {
                        if (distance < distanceToClosestTarget)
                        {
                            targetHeight = collider.bounds.size.y;
                            closestTarget = collider.transform;
                            distanceToClosestTarget = distance;
                        }
                    }
                }
            }
        }

        if (closestTarget != null)
            StartShooting();
    }




    private void StartShooting()
    {
        playerStateController.SetState(PlayerStates.Shooting);

        SetShootingStatus(true);
        Invoke(nameof(Reload), fireRate);
        Invoke(nameof(StopShooting), fireRate);

    }

    public void ShootTheTarget()
    {
        Vector3 targetPos = Vector3.zero;
        if (playerStateController.GetState() != PlayerStates.Shooting)
            return;

        if (closestTarget != null)
            targetPos = closestTarget.position;
        targetPos.y = targetHeight / 3;

        var bullet = bulletPool.Get();
        bullet.SetPlayerResourceController(playerResourceController);
        bullet.Launch(muzzleOfShot.position, targetPos, OnBulletCollision);

        void OnBulletCollision() => bulletPool.Return(bullet);
    }

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private bool IsTargetBehindObstacle(Transform targetTransform, Vector3 directionToTarget, float distance)
    {
        // Check if there's an obstacle between the player and the target
        var playerpos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.2f, playerTransform.position.z);
        if (Physics.Raycast(playerpos, directionToTarget, out RaycastHit hit, distance))
        {
            // Ensure that the hit object is not the target itself
            if (hit.transform != targetTransform)
            {
                Debug.Log($"obstacle is {hit.transform.name}");
                return true;
            }
        }
        Debug.Log($"there is no obstacle");

        return false;
    }

    private void StopShooting() => SetShootingStatus(false);

    private void Reload() => isShooting = false;

    private void SetShootingStatus(bool status)
    {
        isShooting = status;
        animator.SetBool("IsShooting", status);
        SetWeaponStatus(status);
    }

    private void SetWeaponStatus(bool status) => weaponGameobject.SetActive(status);


    public Bullet Preload() => Instantiate(ammoPrefab).GetComponent<Bullet>();
    public void GetAction(Bullet bullet) => bullet.gameObject.SetActive(true);
    public void ReturnAction(Bullet bullet) => bullet.gameObject.SetActive(false);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}