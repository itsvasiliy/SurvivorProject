using Unity.Netcode;
using UnityEngine;

public class PlayerShootingHARDCODED : NetworkBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shootingMuzzle;

    [Header("Player's unit of ammunation")]
    [SerializeField] private GameObject weaponGameobject;
    [SerializeField] private GameObject ammoPrefab;

    [Header("The lenght of the clip will be the fire rate speed")]
    [SerializeField] private AnimationClip shootingAnimClip;
    [SerializeField] private float ammoShotAnimationDelay;

    [Header("Others")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStateController playerStateController;
    [SerializeField] private float shootingRadius;

    public float fireRate;

    private Transform closestTarget;
    private bool isShooting = false;


    private void Start() => fireRate = shootingAnimClip.length;

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

        RotateToTarget(closestTarget.position);

        weaponGameobject.SetActive(true);
        animator.SetBool("IsShooting", isShooting);

        Invoke(nameof(ShootTarget_UseWithDelay), ammoShotAnimationDelay);
        Invoke(nameof(Reload), fireRate);
    }

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }


    private void ShootTarget_UseWithDelay()
    {
        Vector3 targetPos = Vector3.zero;
        if (closestTarget != null)
            targetPos = closestTarget.position;
        else return;

        if (playerStateController.GetState() != PlayerStates.Shooting)
            return;

        ShotTheTarget(shootingMuzzle.position, targetPos);
    }

    private void DeactivateWeapon() => weaponGameobject.SetActive(false);

    private void Reload() => isShooting = false;


    private void ShotTheTarget(Vector3 muzzleOfShot, Vector3 target)
    {
        target.y += 0.3f;
        ammoPrefab.GetComponent<Bullet>().SetTarget(target);
        Instantiate(ammoPrefab, muzzleOfShot, ammoPrefab.transform.rotation);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}
