using Unity.Netcode;
using UnityEngine;

public class EnemyShooting : NetworkBehaviour
{
    [Header("Shooting properties")]
    [SerializeField] protected NetworkObject bullet;
    [SerializeField] protected float shootingRadius;
    [SerializeField] protected Transform muzzleOfShot;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected float reloadingTime;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip attackClip;

    [Header("Enemy")]
    [SerializeField] protected Transform EnemyTransform;

    protected Vector3 detectedPlayer;

    private IEnemyShooting enemyShooting;

    private void Start()
    {
        enemyShooting = GetComponent<IEnemyShooting>();

        Invoke(nameof(PlayerDetector), reloadingTime);

        reloadingTime = attackClip.length;
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

        Invoke(nameof(PlayerDetector), reloadingTime);
    }

    private void StartShooting()
    {
        StartShootingAnimation();
        RotateToTarget(detectedPlayer);
        Invoke("ShootTarget_UseWithDelay", reloadingTime - 0.75f);
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
}
