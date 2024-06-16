using Unity.Netcode;
using UnityEngine;


public delegate void Vector3Delegate(Vector3 position);

public class ShootingTracking : MonoBehaviour
{
    [SerializeField] private GameObject ammoPrefab;

    [SerializeField] private float shootingRadius;
    [SerializeField] private float fireRate = 1.3f;

    [SerializeField] Transform shootingMuzzle;

    public Vector3Delegate rotatorDelegate;

    Transform closestTarget;

    private bool isShooting = false;

    float targetHeight;

    Structure structure;

    private int bulletPoolAmount = 2;

    private ObjectPool<Bullet> bulletPool;


    private void Awake()
    {
        structure = GetComponent<Structure>();

        if (structure.isViewing)
            return;
        bulletPool = new ObjectPool<Bullet>(Preload, GetAction, ReturnAction, bulletPoolAmount);
    }

    private void Update()
    {
        if (structure.isViewing)
            return;

        if (closestTarget != null)
            rotatorDelegate?.Invoke(closestTarget.position);

        if (isShooting == false)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, shootingRadius);

            closestTarget = null;
            float distanceToClosestTarget = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
                {
                    targetHeight = collider.bounds.size.y;
                    if (_aimTarget.IsAlive() && _aimTarget.IsVisible())
                    {
                        float distance = Vector3.Distance(transform.position, collider.transform.position);

                        if (distance < distanceToClosestTarget)
                        {
                            closestTarget = collider.transform;
                            distanceToClosestTarget = distance;
                        }
                    }
                }
            }

            if (closestTarget != null)
            {
                ShotTheTarget(closestTarget.position);
            }
        }
    }


    private void ShotTheTarget(Vector3 targetPosition)
    {
        isShooting = true;
        ShotTheTargetliientRpc(shootingMuzzle.position, targetPosition);

        Invoke(nameof(Reload), fireRate);
    }

    [ClientRpc]
    private void ShotTheTargetliientRpc(Vector3 ammoOrigin, Vector3 targetPosition)
    {
        var bullet = bulletPool.Get();
        bullet.Launch(ammoOrigin, targetPosition, OnBulletCollision);

        void OnBulletCollision() => bulletPool.Return(bullet);
    }

    private void Reload() => isShooting = false;

    public Bullet Preload() => Instantiate(ammoPrefab).GetComponent<Bullet>();
    public void GetAction(Bullet bullet) => bullet.gameObject.SetActive(true);
    public void ReturnAction(Bullet bullet) => bullet.gameObject.SetActive(false);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}
