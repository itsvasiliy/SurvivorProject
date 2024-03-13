using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcheryShooting : EnemyShooting, IEnemyShooting
{
    [SerializeField] private float arrowSpeed;

    public void ShootTheBullet(Transform _bulletTransform, Vector3 _targetPosition)
    {
        _bulletTransform.LookAt(_targetPosition);
    }
}
