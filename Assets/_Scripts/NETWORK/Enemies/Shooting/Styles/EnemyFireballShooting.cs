using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyFireballShooting : EnemyShooting, IEnemyShooting
{
//    NetworkObject

//    private void Update() // test
//    {
//        if (Input.GetKeyUp(KeyCode.T))
//        {
//            ShootTheBullet(muzzleOfShot.position, muzzleOfShot.forward * 4);
//        }
//    }

//    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
//    {
//        SpawnTheBulletServerRpc(muzzleOfShot);
//    }

//    [ServerRpc(RequireOwnership = false)]
//    private void SpawnTheBulletServerRpc(Vector3 muzzleOfShot)
//    {
//        NetworkObject fireballClone = Instantiate(base.bullet.GetComponent<NetworkObject>(), muzzleOfShot, Quaternion.identity);
//        fireballClone.Spawn();
//    }

    [ClientRpc]
    private void SpawnTheBulletClientRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        bullet.GetComponent<Bullet>().SetTarget(target);
        Instantiate(bullet, muzzleOfShot, bullet.transform.rotation);
    }
}