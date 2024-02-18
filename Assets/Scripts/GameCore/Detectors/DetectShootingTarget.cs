using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectShootingTarget : MonoBehaviour
{
    [SerializeField] private Transform centreOfTheSphere;

    [Range(1, 100)] public float detectionRadius;

    private Collider currentTargetCollider;

    private float closestTarget = Mathf.Infinity;

    public event Action targetDetectedEvent;

    public Collider GetCurrentTargetCollider
    {
        get { return currentTargetCollider; }
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(centreOfTheSphere.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
            {
                float distance = Vector3.Distance(centreOfTheSphere.position, collider.transform.position);

                if (distance < closestTarget)
                {
                    closestTarget = distance;
                    currentTargetCollider = collider;
                    TargetDetected();
                }
            }
        }
    }

    private void TargetDetected()
    {
        targetDetectedEvent?.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}