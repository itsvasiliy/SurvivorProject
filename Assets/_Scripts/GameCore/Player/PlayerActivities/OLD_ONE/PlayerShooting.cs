using UnityEngine;
using Unity.Netcode;

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

    private void Start() => weaponScript = weaponGameobject.GetComponent<WeaponBase>();

    private void Update()
    {
        if (!IsOwner) return;

        if (playerStateController.GetState() != PlayerStates.Idle) return;

        if (isShooting == false)
        {
            Collider[] colliders = Physics.OverlapSphere(playerTransform.position, shootingRadius);

            Transform closestTarget = null;
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

            if(closestTarget != null)
            {
                ShotTheTarget(closestTarget.position);
                isShooting = true;
            }
        }
    }

    private void ShotTheTarget(Vector3 targetPosition)
    {
        weaponGameobject.SetActive(true);
        RotateToTarget(targetPosition);
        weaponScript.Shoot(shootingMuzzle.position);
        Invoke(nameof(Reload), weaponScript.fireRate);
    }

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private void DeactivateWeapon() => weaponGameobject.SetActive(false);


    private void Reload() => isShooting = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}