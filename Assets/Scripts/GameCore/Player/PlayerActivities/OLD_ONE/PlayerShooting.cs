using UnityEngine;

[RequireComponent(typeof(DetectShootingTarget))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform bulletMuzzle;

    [SerializeField] private AnimationClip shootingAnimClip;

    [SerializeField] private Bullet bulletPrefab;

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
                playerTransform.LookAt(aimTargetCollider.transform.position);

                Bullet bullet = (Bullet)Instantiate(bulletPrefab, bulletMuzzle.position, Quaternion.identity);
                bullet.transform.LookAt(aimTargetCollider.transform.position);
                bullet.SetTargetPOsition = aimTargetCollider.transform.position;

            }
            else
                aimTargetCollider = null;
            Invoke(nameof(Reload), shootingSpeed);
        }

        if (aimTargetCollider != null)
            Invoke(nameof(ShotTheTarget), shootingSpeed);

    }

    private void Reload() => isShooting = false;


    private void OnDestroy()
    {
        shootingTarget.targetDetectedEvent -= SetTarget;
    }
}