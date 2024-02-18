using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DetectShootingTarget))]
public class PlayerShooting : MonoBehaviour
{
    private DetectShootingTarget shootingTarget;

    private IAimTarget aimTarget;

    private void Start()
    {
        if (TryGetComponent<DetectShootingTarget>(out DetectShootingTarget _shootingTarget))
        {
            shootingTarget = _shootingTarget;
        }

        shootingTarget.targetDetectedEvent += SetTarget;
    }

    private void SetTarget()
    {
        if(shootingTarget.GetCurrentTargetCollider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
        {
            print("Bom");
            _aimTarget.GetDamage();
        }
    }

    private void OnDestroy()
    {
        shootingTarget.targetDetectedEvent -= SetTarget;
    }
}