using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(DetectShootingTarget))]
public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Transform bulletMuzzle;

    [SerializeField] private AnimationClip shootingAnimClip;

    [SerializeField] private NetworkObject bulletPrefab;

    [SerializeField] private PlayerStateController playerStateController;

    [SerializeField] private Transform playerTransform;

    private DetectShootingTarget shootingTarget;

    public Collider aimTargetCollider;

    private float shootingSpeed;

    private bool isShooting = false;

    private void Start()
    {
        if (TryGetComponent<DetectShootingTarget>(out DetectShootingTarget _shootingTarget))
        {
            shootingTarget = _shootingTarget;
        }

        shootingSpeed = shootingAnimClip.length;
        shootingTarget.targetDetectedEvent += SetTarget;
    }

    private void SetTarget()
    {
        aimTargetCollider = shootingTarget.GetCurrentTargetCollider;
        ShotTheTarget();
    }

    private void ShotTheTarget()
    {
        if ( playerStateController.GetState() == PlayerStates.Idle &&
            aimTargetCollider != null && isShooting == false)
        {
            isShooting = true;
            float distance = Vector3.Distance(playerTransform.position, aimTargetCollider.transform.position);

            if (distance < shootingTarget.detectionRadius)
            {
                Vector3 relativePosition = aimTargetCollider.transform.position - playerTransform.position;
                Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
                playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

               if(IsOwner) SpawnTheBulletServerRpc();

                //NetworkObject bullet = (NetworkObject)Instantiate(bulletPrefab, bulletMuzzle.position, Quaternion.identity);
                //bullet.Spawn();

                //bullet.transform.LookAt(aimTargetCollider.transform.position);
                //bullet.GetComponent<Bullet>().SetTargetPOsition = aimTargetCollider.transform.position;

            }
            else
                aimTargetCollider = null;
            Invoke(nameof(Reload), shootingSpeed);
        }

        if (aimTargetCollider != null)
            Invoke(nameof(ShotTheTarget), shootingSpeed);

    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnTheBulletServerRpc()
    {
        NetworkObject bullet = (NetworkObject)Instantiate(bulletPrefab, bulletMuzzle.position, Quaternion.identity);
        bullet.Spawn();
    }

    private void Reload() => isShooting = false;


    private void OnDestroy()
    {
        shootingTarget.targetDetectedEvent -= SetTarget;
    }
}