using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemyShooting
{
    public void ShootTheBullet(Transform _bulletTransform, Vector3 _targetPosition);
}