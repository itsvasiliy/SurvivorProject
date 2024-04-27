using UnityEngine;

public class PlayerShootingHARDCODED : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shootingMuzzle;

    [Header("Player's shoot properties")]
    [SerializeField] private GameObject weaponGameobject;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private float attacksPerSecond;
    [SerializeField] private float shootingRadius;

    [Header("Others")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStateController playerStateController;

    private Transform closestTarget;
    private bool isShooting = false;

    private float fireRate;
    private float targetHeight;


    private void Start()
    {
        animator.SetFloat("AttackSpeed", attacksPerSecond);
        fireRate = 1 / attacksPerSecond;
    }


    private void Update()
    {
        var playerState = playerStateController.GetState();
        if (playerState != PlayerStates.Idle &&
            playerState != PlayerStates.Shooting) return;
        if (isShooting) return;

        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, shootingRadius);

        closestTarget = null;
        float distanceToClosestTarget = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
            {
                if (_aimTarget.IsEnabled())
                {
                    float distance = Vector3.Distance(playerTransform.position, collider.transform.position);

                    if (distance < distanceToClosestTarget)
                    {
                        targetHeight = collider.bounds.size.y;
                        closestTarget = collider.transform;
                        distanceToClosestTarget = distance;
                    }
                }

            }
        }

        if (closestTarget != null)
            ShotTheTarget();
        else
            StopShooting();
    }

    private void StopShooting()
    {
        isShooting = false;
        animator.SetBool("IsShooting", isShooting);
        Invoke(nameof(DeactivateWeapon), 0.15f);
    }


    private void ShotTheTarget()
    {
        isShooting = true;
        playerStateController.SetState(PlayerStates.Shooting);
        weaponGameobject.SetActive(true);
        animator.SetBool("IsShooting", isShooting);

        //  Invoke(nameof(ShootTarget_UseWithDelay), ammoShotAnimationDelay);
        //shot calls in animation BowShotLooped

        RotateToTarget(closestTarget.position);
        Invoke(nameof(Reload), fireRate);
    }

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }


    public void ShootTarget_UseWithDelay()
    {
        Vector3 targetPos = Vector3.zero;
        if (closestTarget != null)
            targetPos = closestTarget.position;
        else return;

        if (playerStateController.GetState() != PlayerStates.Shooting)
            return;

        targetPos.y = targetHeight / 3;
        InstantiateAmmo(shootingMuzzle.position, targetPos);
    }

    private void DeactivateWeapon() => weaponGameobject.SetActive(false);

    private void Reload() => isShooting = false;


    private void InstantiateAmmo(Vector3 muzzleOfShot, Vector3 target)
    {
        target.y += 0.6f;
        ammoPrefab.GetComponent<Bullet>().SetTarget(target);
        Instantiate(ammoPrefab, muzzleOfShot, ammoPrefab.transform.rotation);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}
