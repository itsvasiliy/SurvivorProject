using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyShooting : NetworkBehaviour
{
    [SerializeField] private NetworkObject bullet;

    [SerializeField] private Transform EnemyTransform;

    [SerializeField] private float shootingRadius;
    [SerializeField] private float reloadingTime;
    [SerializeField] private float lifeTime;

    private Transform detectedPlayer;

    private IEnemyShooting enemyShooting;

    private void Start()
    {
        enemyShooting = GetComponent<IEnemyShooting>();

        Invoke(nameof(PlayerDetector), reloadingTime);
    }

    private void PlayerDetector()
    {
        Collider[] colliders = Physics.OverlapSphere(EnemyTransform.position, shootingRadius);

        float closestDistance = float.MaxValue;
        Collider closestCollider = null;

        foreach (Collider collider in colliders)
        {
            PlayerStateController aimTarget = collider.GetComponent<PlayerStateController>();

            if (aimTarget != null)
            {
                float distance = Vector3.Distance(EnemyTransform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }

        if(closestCollider != null)
        {
            Vector3 spawnOrigin = transform.position;
            spawnOrigin.y += 5f;

            detectedPlayer = closestCollider.transform;
            SpawnBulletServerRpc(spawnOrigin, detectedPlayer.position);
        }

        Invoke(nameof(PlayerDetector), reloadingTime);
    }

    [ServerRpc(RequireOwnership = false)]
    protected void SpawnBulletServerRpc(Vector3 spanwOrigin,Vector3 _targetPosition)
    {
        NetworkObject bulletClone = Instantiate(bullet, spanwOrigin, Quaternion.identity);
        bulletClone.Spawn();

        GetComponent<IEnemyShooting>().ShootTheBullet(bulletClone.transform, _targetPosition);
    }
}
