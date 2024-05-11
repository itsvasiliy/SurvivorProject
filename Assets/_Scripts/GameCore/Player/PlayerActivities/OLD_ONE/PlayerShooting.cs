using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
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
                if (_aimTarget.IsEnabled()) // means _aimTarget is alive
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
            StartShooting();
        else
            StopShooting();
    }

    private void StartShooting()
    {
        SetShootingStatus(true);

        //ShootTheTarget() calls in animation to avoid calculating ShootTheTarget delay
        //for each attack speed and attack aborting
        Invoke(nameof(Reload), fireRate);
    }


    // this calls in animation event to avoid calculating ShootTheTarget delay for each attack speed and attack aborting
    public void ShootTheTarget()
    {
        Vector3 targetPos = Vector3.zero;
        if (closestTarget != null)
            targetPos = closestTarget.position;
        else return;

        if (playerStateController.GetState() != PlayerStates.Shooting)
            return;

        targetPos.y = targetHeight / 3;

       var bulletSetter =  ammoPrefab.GetComponent<Bullet>();
        bulletSetter.SetTarget(targetPos);
        bulletSetter.SetPlayerResourceController(playerResourceController);
        Instantiate(ammoPrefab, muzzleOfShot.position, ammoPrefab.transform.rotation);
    }


    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private void StopShooting() => SetShootingStatus(false);

    private void Reload() => isShooting = false;

    private void SetShootingStatus(bool status)
    {
        isShooting = status;
        animator.SetBool("IsShooting", status);
       SetWeaponStatusClientRpc(status);

        if (status == true)
            playerStateController.SetState(PlayerStates.Shooting);
    }

    [ClientRpc]
    private void SetWeaponStatusClientRpc(bool status) => weaponGameobject.SetActive(status);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}