using Unity.Netcode;
using UnityEngine;

public class EnemyFireballShooting : EnemyShooting, IEnemyShooting
{
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        SpawnTheBulletClientRpc(muzzleOfShot, _targetPosition);
    }

    [ClientRpc]
    private void SpawnTheBulletClientRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        var bullet = bulletPool.Get();
        bullet.Launch(muzzleOfShot, target, OnBulletCollision);

        void OnBulletCollision() => bulletPool.Return(bullet);
    }
}