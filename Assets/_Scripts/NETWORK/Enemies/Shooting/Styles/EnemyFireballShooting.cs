using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyFireballShooting : EnemyShooting, IEnemyShooting
{
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        SpawnTheBulletClientRpc(muzzleOfShot, _targetPosition);
    }

    [ClientRpc]
    private void SpawnTheBulletClientRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        bullet.GetComponent<Bullet>().SetTarget(target);
        Instantiate(bullet, muzzleOfShot, bullet.transform.rotation);
    }
}