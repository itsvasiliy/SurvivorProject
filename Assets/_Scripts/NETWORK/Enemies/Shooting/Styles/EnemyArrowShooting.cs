using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyArrowShooting : EnemyShooting, IEnemyShooting
{
    public void ShootTheBullet(Transform _bulletTransform, Vector3 _targetPosition)
    {
        ShotTheTargetServerRpc(transform.position, _targetPosition);
    }


    [ServerRpc(RequireOwnership = false)]
    private void ShotTheTargetServerRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        NetworkObject ammo = Instantiate(bullet, muzzleOfShot, bullet.transform.rotation);
        ammo.GetComponent<Bullet>().SetTarget(target);
        ammo.Spawn();
    }
}
