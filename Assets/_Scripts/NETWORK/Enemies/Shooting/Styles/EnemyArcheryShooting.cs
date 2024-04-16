using Unity.Netcode;
using UnityEngine;

public class EnemyArcheryShooting : EnemyShooting, IEnemyShooting
{
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        ShotTheTargetClientRpc(muzzleOfShot, _targetPosition);
    }


    [ClientRpc]
    private void ShotTheTargetClientRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        bullet.GetComponent<Bullet>().SetTarget(target);
        Instantiate(bullet, muzzleOfShot, bullet.transform.rotation);
    }
}
