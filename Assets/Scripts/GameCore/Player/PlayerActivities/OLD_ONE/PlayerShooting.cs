using UnityEngine;
using Unity.Netcode;

public class PlayerShooting : NetworkBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform shootingMuzzle;

    [Header("Player's unit of ammunation")]
    [SerializeField] private NetworkObject ammoPrefab;

    [Header("The lenght of the clip will be the fire rate speed")]
    [SerializeField] private AnimationClip shootingAnimClip;

    [Header("The shooting distance")]
    [SerializeField] private float shootingRadius;

    private float fireRate;

    private bool isShooting = false;

    private void Start()
    {
        if (!IsOwner) Destroy(this);

        fireRate = shootingAnimClip.length;
    }

    void Update()
    {
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
        Vector3 relativePosition = targetPosition - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

        print(targetPosition);

        Invoke(nameof(Reload), fireRate);
    }

    private void Reload() => isShooting = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }


    ///// <summary>
    /////
    ///// 
    ///// </summary>
    //[SerializeField] private Transform bulletMuzzle;

    //[SerializeField] private AnimationClip shootingAnimClip;

    //[SerializeField] private NetworkObject bulletPrefab;

    //[SerializeField] private PlayerStateController playerStateController;

    //[SerializeField] private Transform playerTransformOLD;

    //private DetectShootingTarget shootingTarget;

    //public Collider aimTargetCollider;

    //private float shootingSpeed;

    ////private bool isShooting = false;

    //private void Start1()
    //{
    //    if (!IsOwner)
    //    {
    //        Destroy(shootingTarget);
    //        Destroy(this);
    //    }

    //    if (TryGetComponent<DetectShootingTarget>(out DetectShootingTarget _shootingTarget))
    //    {
    //        shootingTarget = _shootingTarget;
    //    }

    //    shootingSpeed = shootingAnimClip.length;
    //    shootingTarget.targetDetectedEvent += SetTarget;
    //}

    //private void SetTarget()
    //{
    //    aimTargetCollider = shootingTarget.GetCurrentTargetCollider;
    //    ShotTheTarget();
    //}

    //private void ShotTheTarget()
    //{
    //    if (playerStateController.GetState() == PlayerStates.Idle &&
    //        aimTargetCollider != null && isShooting == false)
    //    {
    //        isShooting = true;
    //        float distance = Vector3.Distance(playerTransform.position, aimTargetCollider.transform.position);

    //        if (distance < shootingTarget.detectionRadius)
    //        {
    //            Vector3 relativePosition = aimTargetCollider.transform.position - playerTransform.position;
    //            Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
    //            playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

    //            print("Spanw of bullet");
    //            SpawnTheBulletServerRpc(bulletMuzzle.position);
    //        }
    //        else
    //        {
    //            aimTargetCollider = null;
    //        }

    //        Invoke(nameof(Reload), shootingSpeed);
    //    }

    //    if (aimTargetCollider != null)
    //        Invoke(nameof(ShotTheTarget), shootingSpeed);

    //}

    //[ServerRpc(RequireOwnership = false)]
    //private void SpawnTheBulletServerRpc(Vector3 bulletMuzzlePosition)
    //{
    //    print("Here");
    //    NetworkObject bullet = (NetworkObject)Instantiate(bulletPrefab, bulletMuzzlePosition, Quaternion.identity);
    //    bullet.Spawn();
    //}


    //private void Reload() => isShooting = false;


    //public override void OnDestroy()
    //{
    //    shootingTarget.targetDetectedEvent -= SetTarget;
    //}
}