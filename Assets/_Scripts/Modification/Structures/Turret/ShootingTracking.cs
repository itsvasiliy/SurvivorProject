using Unity.Netcode;
using UnityEngine;


public delegate void Vector3Delegate(Vector3 position);

public class ShootingTracking : MonoBehaviour
{
    [SerializeField] private NetworkObject ammoPrefab;

    [SerializeField] private float shootingRadius;
    [SerializeField] private float fireRate = 1.3f;

    [SerializeField] Transform shootingMuzzle;

    public Vector3Delegate rotatorDelegate;

    Transform closestTarget;
    private bool isShooting = false;


    private void Update()
    {
        if(closestTarget != null)
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
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    if (distance < distanceToClosestTarget)
                    {
                        closestTarget = collider.transform;
                        distanceToClosestTarget = distance;
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
        ShotTheTargetServerRpc(shootingMuzzle.position, targetPosition);

        Invoke(nameof(Reload), fireRate);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShotTheTargetServerRpc(Vector3 ammoOrigin, Vector3 targetPosition)
    {
        var ammoScript = ammoPrefab.GetComponent<Bullet>();
        ammoScript.SetTarget(targetPosition);
        NetworkObject ammo = Instantiate(ammoPrefab, ammoOrigin, Quaternion.identity);
        ammo.Spawn();
    }

    private void Reload() => isShooting = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}
