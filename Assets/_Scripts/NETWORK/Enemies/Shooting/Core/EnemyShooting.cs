using Unity.Netcode;
using UnityEngine;

public class EnemyShooting : NetworkBehaviour
{
    [Header("Shooting properties")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float shootingRadius;
    [SerializeField] protected Transform muzzleOfShot;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip attackClip;
    [SerializeField] protected float bulletSpawnDelay;

    [Header("Enemy")]
    [SerializeField] protected Transform EnemyTransform;

    [SerializeField] private EnemyMovement movement;


    protected Transform detectedPlayer;

    private IEnemyShooting shootingStyle;

    private Collider closestCollider;

    protected float reloadingTime;



    private void Start()
    {
        if (IsServer == false)
            return;

        shootingStyle = GetComponent<IEnemyShooting>();
        reloadingTime = attackClip.length;
    }

    private void Update()
    {
        if (closestCollider != null)
            transform.LookAt(closestCollider.transform);
    }

    private void PlayerDetector()
    {
        Collider[] colliders = Physics.OverlapSphere(EnemyTransform.position, shootingRadius);

        float closestDistance = float.MaxValue;
        closestCollider = null;

        foreach (Collider collider in colliders)
        {
            PlayerHealthHandlerForController aimTarget = collider.GetComponent<PlayerHealthHandlerForController>();

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
            detectedPlayer = closestCollider.transform;
            StartShooting();
        }
    }

    private void StartShooting()
    {
        if (movement != null)
            movement.SetCanMoveStatus(false);
        StartShootingAnimation();
        RotateToTarget(detectedPlayer.position);
        Invoke(nameof(ShootTarget_UseWithDelay), bulletSpawnDelay);
     //   Invoke(nameof(StopShooting), reloadingTime);
    }

    private void StopShooting()
    {
        StopShootingAnimation();
        if (movement != null)
            movement.SetCanMoveStatus(true);
    }

    private void StartShootingAnimation() => animator.SetBool("Attack", true);
    private void StopShootingAnimation() => animator.SetBool("Attack", false);

    private void ShootTarget_UseWithDelay()
    {
        var targetPos = detectedPlayer.position;
        targetPos.y += 0.7f;
        shootingStyle.ShootTheBullet(muzzleOfShot.position, targetPos);
    }


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
