using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shootingMuzzle;

    [Header("PlayerController")]
    [SerializeField] private PlayerStateController playerStateController;

    [Header("Player's unit of ammunation")]
    [SerializeField] private GameObject weaponGameobject;

    [Header("The lenght of the clip will be the fire rate speed")]
    [SerializeField] private AnimationClip shootingAnimClip;

    [SerializeField] private Animator animator;

    [Header("The shooting distance")]
    [SerializeField] private float shootingRadius;

    private bool isShooting = false;

    private WeaponBase weaponScript;
    private Transform closestTarget;
    public float fireRate;


    private void Start() => fireRate = shootingAnimClip.length;

    private void Update()
    {
        if (!IsOwner) return;
        if (playerStateController.GetState() != PlayerStates.Idle) return;
        if (isShooting) return;

        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, shootingRadius);

        closestTarget = null;
        float distanceToClosestTarget = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);

                if (distance < distanceToClosestTarget)
                {
                    closestTarget = collider.transform;
                    distanceToClosestTarget = distance;
                }
            }
        }

        if (closestTarget != null)
            ShotTheTarget(closestTarget.position);
        else
            StopShooting();
    }

    private void StopShooting()
    {
        isShooting = false;
        animator.SetBool("IsShooting", isShooting);
        Invoke("DeactivateWeapon", 0.15f);
    }


    private void ShotTheTarget(Vector3 targetPosition)
    {
        isShooting = true;
        RotateToTarget(targetPosition);

        weaponGameobject.SetActive(true);
        if (weaponScript == null)
            weaponScript = weaponGameobject.GetComponent<WeaponBase>();

        ShotTheTargetServerRpc(shootingMuzzle.position);  // !! weaponScript.Shoot(shootingMuzzle.position); !!

        animator.SetBool("IsShooting", isShooting);

        Invoke(nameof(Reload), fireRate);   // Invoke(nameof(Reload), weaponScript.fireRate);
    }

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private void DeactivateWeapon() => weaponGameobject.SetActive(false);

    private void Reload() => isShooting = false;



    [ServerRpc(RequireOwnership = false)]
    private void ShotTheTargetServerRpc(Vector3 muzzleOfShot) //tmp solution
    {
        NetworkObject ammo = Instantiate(weaponScript.ammoPrefab, muzzleOfShot,
            weaponScript.ammoPrefab.transform.rotation);
        ammo.Spawn();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}