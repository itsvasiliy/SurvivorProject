using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IDamageableDetectionCube : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform detectionCubeCentre;

    [Range(0, 10)] public float detectionRange;

    public Color gizmoColor = Color.red;

    public event Action detected;

    private RaycastHit[] hits;

    public RaycastHit[] GetHits
    {
        get { return hits; }
    }

    private void Update()
    {
        hits = Physics.BoxCastAll(_transform.position, Vector3.one * 0.5f, _transform.forward, _transform.rotation, detectionRange);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable _damageable))
                {
                    DamageableDetected();
                }
            }
        }
    }

    private void DamageableDetected()
    {
        detected?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(detectionCubeCentre.position, Vector3.one * detectionRange);
    }
}