using Unity.Netcode;
using UnityEngine;

public abstract class WeaponBase : NetworkBehaviour
{
    [SerializeField] private NetworkObject ammoPrefab;

    [Header("Animations")]
    [SerializeField] private AnimationClip weaponShootAnimClip;

    public float fireRate = 1.7f;

   // private void Start() => fireRate = weaponShootAnimClip.length;
   
    public virtual void PlayAnimation()
    {
      //  playerShootAnimation.play
    }

    public virtual void Shoot(Vector3 muzzleOfShot)
    {
        ShotTheTargetServerRpc(muzzleOfShot);
    }


    [ServerRpc(RequireOwnership = false)]
    private void ShotTheTargetServerRpc(Vector3 muzzleOfShot)
    {
        NetworkObject ammo = Instantiate(ammoPrefab, muzzleOfShot, Quaternion.identity);
        ammo.Spawn();
    }
}
