using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyShooting : NetworkBehaviour
{
    [SerializeField] private NetworkObject bullet;

    [SerializeField] private float lifeTime;

    [ServerRpc(RequireOwnership = false)]
    protected void SpawnBulletServerRpc(Vector3 spanwOrigin)
    {
        NetworkObject bulletClone = Instantiate(bullet, spanwOrigin, Quaternion.identity);
        bulletClone.Spawn();
    }
}
