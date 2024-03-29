using Unity.Netcode;
using UnityEngine;

public class PlayerShootingHARDCODED : NetworkBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shootingMuzzle;

    [Header("Player's unit of ammunation")]
    [SerializeField] private GameObject weaponGameobject;
    [SerializeField] private NetworkObject ammoPrefab;

    [Header("The lenght of the clip will be the fire rate speed")]
    [SerializeField] private AnimationClip shootingAnimClip;

    [SerializeField] private Animator animator;

    [SerializeField] private PlayerStateController playerStateController;

    [SerializeField] private float shootingRadius;

    public float fireRate;

    private Transform closestTarget;
    private bool isShooting = false;


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

        ShotTheTargetServerRpc(shootingMuzzle.position, closestTarget.position);

        animator.SetBool("IsShooting", isShooting);

        Invoke(nameof(Reload), fireRate);
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
    private void ShotTheTargetServerRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        NetworkObject ammo = Instantiate(ammoPrefab, muzzleOfShot,
            ammoPrefab.transform.rotation);
        ammo.GetComponent<Bullet>().SetTarget(target);
        ammo.Spawn();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}
