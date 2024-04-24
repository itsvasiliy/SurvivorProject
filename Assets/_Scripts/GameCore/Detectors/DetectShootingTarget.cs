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

    private float distanceSpread = 1.5f;

    public event Action targetDetectedEvent;

    public Collider GetCurrentTargetCollider
    {
        get { return currentTargetCollider; }
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(centreOfTheSphere.position, detectionRadius - distanceSpread);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IAimTarget>(out IAimTarget _aimTarget))
            {
                float distance = Vector3.Distance(centreOfTheSphere.position, collider.transform.position);

                if (currentTargetCollider != null)
                {
                    closestTarget = Vector3.Distance(centreOfTheSphere.position, currentTargetCollider.transform.position);
                }

                if (distance < closestTarget)
                {
                    print("Closest one is " + collider.name);

                    closestTarget = distance;
                    currentTargetCollider = collider;
                    TargetDetected();
                    break;
                }
            }
        }
    }

    private void TargetDetected()
    {
        targetDetectedEvent?.Invoke();
    }

    public void resetCurrentTarget()
    {
        currentTargetCollider = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}