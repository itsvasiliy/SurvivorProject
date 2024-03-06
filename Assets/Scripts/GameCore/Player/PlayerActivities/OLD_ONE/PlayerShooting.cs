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

        ShotTheTargetServerRpc(shootingMuzzle.position);

        Invoke(nameof(Reload), fireRate);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShotTheTargetServerRpc(Vector3 ammoOrigin)
    {
        NetworkObject ammo = (NetworkObject)Instantiate(ammoPrefab, ammoOrigin, Quaternion.identity);
        ammo.Spawn();
    }


    private void Reload() => isShooting = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, shootingRadius);
    }
}