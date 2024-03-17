using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyArcheryShooting : EnemyShooting, IEnemyShooting
{
    [SerializeField] private float arrowSpeed;

    [SerializeField] private int arrowDamage;

    private bool isExplode = false;

    public void ShootTheBullet(Transform _bulletTransform, Vector3 _targetPosition)
    {
        StartCoroutine(FlyTowardsTarget(_bulletTransform, _targetPosition));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<NetworkObjectHealth>(out NetworkObjectHealth _networkObjectHealth))
        {
            _networkObjectHealth.GetDamage(arrowDamage);
        }

        isExplode = true;
    }

    private IEnumerator FlyTowardsTarget(Transform _bulletTransform, Vector3 _targetPosition)
    {
        while (isExplode)
        {
            Vector3 direction = _targetPosition - _bulletTransform.position;
            direction.Normalize();
            _bulletTransform.Translate(direction * arrowSpeed * Time.deltaTime, Space.World);

            _bulletTransform.LookAt(_targetPosition);

            yield return new WaitForFixedUpdate(); 
        }

        _bulletTransform.GetComponent<NetworkObject>().Despawn();
    }
}
