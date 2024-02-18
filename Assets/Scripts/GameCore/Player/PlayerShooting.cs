using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DetectShootingTarget))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private AnimationClip shootingAnimClip;

    private DetectShootingTarget shootingTarget;

    private IAimTarget aimTarget;

    private float shootingSpeed;

    private void Start()
    {
        if (TryGetComponent<DetectShootingTarget>(out DetectShootingTarget _shootingTarget))
        {
            shootingTarget = _shootingTarget;
        }

        shootingSpeed = shootingAnimClip.length;

        shootingTarget.targetDetectedEvent += SetTarget;
    }

    private void SetTarget()
    {
        if(shootingTarget.GetCurrentTargetCollider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
        {
            _aimTarget.GetDamage();
        }
    }

    private void OnDestroy()
    {
        shootingTarget.targetDetectedEvent -= SetTarget;
    }
}