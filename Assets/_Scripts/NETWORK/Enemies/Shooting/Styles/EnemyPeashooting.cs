using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemyPeashooting : EnemyShooting, IEnemyShooting
{
    [SerializeField] float deviationAngle;
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        StartCoroutine(ShootAreaWithDelay(muzzleOfShot, _targetPosition));
    }


    [ClientRpc]
    private void ShotTheTargetClientRpc(Vector3 muzzleOfShot, Vector3 target)
    {
        bullet.GetComponent<Bullet>().SetTarget(target);
        Instantiate(bullet, muzzleOfShot, bullet.transform.rotation);
    }

    IEnumerator ShootAreaWithDelay(Vector3 muzzleOfShot, Vector3 _targetPosition)
    {
        ShotTheTargetClientRpc(muzzleOfShot, _targetPosition);
        yield return new WaitForSeconds(0.2f);

        ShotTheTargetClientRpc(muzzleOfShot, GetDeviateTarget(_targetPosition));
        yield return new WaitForSeconds(0.2f);

        ShotTheTargetClientRpc(muzzleOfShot, GetDeviateTarget(_targetPosition));
    }

    Vector3 GetDeviateTarget(Vector3 originalTarget)
    {
        Vector2 randomDeviation = Random.insideUnitCircle * deviationAngle;
        Vector3 deviatedTarget = originalTarget + new Vector3(randomDeviation.x, 0f, randomDeviation.y);
        return deviatedTarget;
    }

}