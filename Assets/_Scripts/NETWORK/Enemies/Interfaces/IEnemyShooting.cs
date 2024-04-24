using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemyShooting
{
    public void ShootTheBullet(Vector3 muzzleOfShot, Vector3 _targetPosition);
}