using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DetectShootingTarget))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private AnimationClip shootingAnimClip;

    private DetectShootingTarget shootingTarget;

    private Transform playerTransform;

    private Collider aimTargetCollider;

    private float shootingSpeed;

    private bool isShooting = false;

    private void Start()
    {
        if (TryGetComponent<DetectShootingTarget>(out DetectShootingTarget _shootingTarget))
        {
            shootingTarget = _shootingTarget;
        }

        shootingSpeed = shootingAnimClip.length;

        playerTransform = GetComponent<Transform>();

        shootingTarget.targetDetectedEvent += SetTarget;
    }

    private void SetTarget()
    {
        aimTargetCollider = shootingTarget.GetCurrentTargetCollider;
        ShotTheTarget();
    }

    private void ShotTheTarget()
    {
        if(aimTargetCollider != null)
        {
            float distance = Vector3.Distance(playerTransform.position, aimTargetCollider.transform.position);

            if(distance < shootingTarget.detectionRadius)
            {
                Invoke(nameof(ShotTheTarget), shootingSpeed);
            }
        }
    }

    private void OnDestroy()
    {
        shootingTarget.targetDetectedEvent -= SetTarget;
    }
}