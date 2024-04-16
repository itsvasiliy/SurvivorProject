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


    private void Update()
    {
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
        ShotTheTargetliientRpc(shootingMuzzle.position, targetPosition);

        Invoke(nameof(Reload), fireRate);
    }

    [ClientRpc]
    private void ShotTheTargetliientRpc(Vector3 ammoOrigin, Vector3 targetPosition)
    {
        ammoPrefab.GetComponent<Bullet>().SetTarget(targetPosition);
        Instantiate(ammoPrefab, ammoOrigin, Quaternion.identity);
    }

    private void Reload() => isShooting = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}
