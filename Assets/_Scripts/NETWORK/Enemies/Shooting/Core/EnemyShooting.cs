using Unity.Netcode;
using UnityEngine;

public class EnemyShooting : NetworkBehaviour
{
    [Header("Shooting properties")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float shootingRadius;
    [SerializeField] protected Transform muzzleOfShot;
    [SerializeField] protected float reloadingTime;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip attackClip;
    [SerializeField] protected float bulletSpawnDelay;

    [Header("Enemy")]
    [SerializeField] protected Transform EnemyTransform;

    protected Vector3 detectedPlayer;

    private IEnemyShooting enemyShooting;

    private void Start()
    {
        if (IsServer == false)
            return;

        enemyShooting = GetComponent<IEnemyShooting>();
        reloadingTime = attackClip.length;

        InvokeRepeating(nameof(PlayerDetector), 0f, reloadingTime);
    }


    private void PlayerDetector()
    {
        Collider[] colliders = Physics.OverlapSphere(EnemyTransform.position, shootingRadius);

        float closestDistance = float.MaxValue;
        Collider closestCollider = null;

        foreach (Collider collider in colliders)
        {
            PlayerHealthController aimTarget = collider.GetComponent<PlayerHealthController>();

            if (aimTarget != null && aimTarget.enabled) //aimTarget.enabled means player is alive
            {
                float distance = Vector3.Distance(EnemyTransform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
            else
            {
                StopShooting();
            }
        }

        if (closestCollider != null)
        {
            Vector3 spawnOrigin = transform.position;
            spawnOrigin.y += 5f;
            detectedPlayer = new Vector3(closestCollider.transform.position.x,
                closestCollider.transform.position.y + 1.8f, closestCollider.transform.position.z);

            StartShooting();
        }
    }

    private void StartShooting()
    {
        StartShootingAnimation();
        RotateToTarget(detectedPlayer);
        Invoke("ShootTarget_UseWithDelay", bulletSpawnDelay);
    }

    private void StopShooting() => animator.SetBool("Attack", false);
    private void StartShootingAnimation() => animator.SetBool("Attack", true);

    private void ShootTarget_UseWithDelay() =>
                GetComponent<IEnemyShooting>().ShootTheBullet(muzzleOfShot.position, detectedPlayer);


    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - EnemyTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        EnemyTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }

    private void OnEnable() => InvokeRepeating(nameof(PlayerDetector), 0f, reloadingTime);

    private void OnDisable()
    {
        CancelInvoke(nameof(PlayerDetector));
        CancelInvoke(nameof(ShootTarget_UseWithDelay));
        StopShooting();
    }

}
