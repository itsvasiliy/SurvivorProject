using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GolemAreaShooting : EnemyShooting, IEnemyShooting
{
    [SerializeField] private List<Transform> positionsToShot;


    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        foreach (var target in positionsToShot)
            SpawnBulletClientRpc(muzzleOfShot, target.position);
    }


    [ClientRpc]
    private void SpawnBulletClientRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        var bullet = bulletPool.Get();
        bullet.Launch(muzzleOfShot, target, OnBulletCollision);

        void OnBulletCollision() => bulletPool.Return(bullet);
    }
}
