using Unity.Netcode;
using UnityEngine;

public class EnemyArcheryShooting : EnemyShooting, IEnemyShooting
{
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        ShotTheTargetServerRpc(muzzleOfShot, _targetPosition);
    }


    [ServerRpc(RequireOwnership = false)]
    private void ShotTheTargetServerRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        NetworkObject ammo = Instantiate(bullet, muzzleOfShot, bullet.transform.rotation);
        ammo.GetComponent<Bullet>().SetTarget(target);
        ammo.Spawn();
    }
}
