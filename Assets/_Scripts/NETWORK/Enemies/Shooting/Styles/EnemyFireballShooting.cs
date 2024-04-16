using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyFireballShooting : EnemyShooting, IEnemyShooting
{
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        SpawnTheBulletServerRpc(muzzleOfShot);


    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnTheBulletServerRpc(Vector3 muzzleOfShot)
    {
        NetworkObject fireballClone = Instantiate(base.bullet, muzzleOfShot, Quaternion.identity);
        fireballClone.Spawn();
    }
}