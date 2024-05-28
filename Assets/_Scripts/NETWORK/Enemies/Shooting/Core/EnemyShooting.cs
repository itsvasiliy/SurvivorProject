using Unity.Netcode;
using UnityEngine;

public class EnemyShooting : NetworkBehaviour
{
    [Header("Shooting properties")]
    [SerializeField] protected GameObject bullet;

    [SerializeField] private int bulletPoolAmount;

    [SerializeField] protected float shootingRadius;

    [SerializeField] protected Transform muzzleOfShot;


    [Header("Animation")]
    [SerializeField] protected Animator animator;

    [SerializeField] protected AnimationClip attackClip;

    [SerializeField] protected float bulletSpawnDelay;
    [SerializeField] protected float reloadingTime;


    [Header("Enemy")]
    [SerializeField] protected Transform EnemyTransform;

    [SerializeField] private EnemyMovement movement;


    protected Transform detectedPlayer;
    
    protected ObjectPool<Bullet> bulletPool;

    private IEnemyShooting shootingStyle;

    private Collider closestCollider;


    private bool isReloading = false;
    private bool isShooting = false;


    private void Awake() => bulletPool = new ObjectPool<Bullet>(Preload, GetAction, ReturnAction, bulletPoolAmount);


    private void Start()
    {
        if (IsServer == false)
            return;

        shootingStyle = GetComponent<IEnemyShooting>();
    }

    private void Update()
    {
        if (closestCollider != null)
            transform.LookAt(closestCollider.transform);
    }

    private void PlayerDetector()
    {
        if (isReloading)
            return;

        Collider[] colliders = Physics.OverlapSphere(EnemyTransform.position, shootingRadius);

        float closestDistance = float.MaxValue;
        closestCollider = null;

        foreach (Collider collider in colliders)
        {
            PlayerHealthHandlerForController aimTarget = collider.GetComponent<PlayerHealthHandlerForController>();

            if (aimTarget != null && aimTarget.IsAlive())
            {
                float distance = Vector3.Distance(EnemyTransform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
            else if(isShooting)
            {
                StopShooting();
                return;
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
        Invoke(nameof(Reload), reloadingTime);

        isShooting = true;
        isReloading = true;

        Invoke(nameof(StopShooting), attackClip.length); //if reloading is 4 seconds, shooter dont need to be in shooting state all time so disabling it
    }

    private void StopShooting()
    {
        StopShootingAnimation();
        if (movement != null)
            movement.SetCanMoveStatus(true);
        isShooting = false;
    }

    private void StartShootingAnimation() => animator.SetBool("Attack", true);
    private void StopShootingAnimation() => animator.SetBool("Attack", false);
    private void Reload() => isReloading = false;

    private void ShootTarget_UseWithDelay()
    {
        if (!IsOwner)
            return;

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



    public Bullet Preload() => Instantiate(bullet).GetComponent<Bullet>();
    public void GetAction(Bullet bullet) => bullet.gameObject.SetActive(true);
    public void ReturnAction(Bullet bullet) => bullet.gameObject.SetActive(false);


    private void OnEnable() => InvokeRepeating(nameof(PlayerDetector), 0f, 0.2f);

    private void OnDisable()
    {
        CancelInvoke(nameof(PlayerDetector));
        CancelInvoke(nameof(ShootTarget_UseWithDelay));
        StopShooting();
    }

}
