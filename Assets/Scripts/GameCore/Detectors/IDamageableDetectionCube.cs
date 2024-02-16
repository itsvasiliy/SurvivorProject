using System;
using UnityEngine;
using Zenject;

public class IDamageableDetectionCube : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform detectionCubeCentre;

    [Range(0, 10)] public float detectionRange;

    public Color gizmoColor = Color.red;

    public event Action detected;
    public event Action leftDetectedAction;

    private RaycastHit[] hits;

    private bool playerInDetectionArea = false;

    [Inject] IPlayerStateController playerStateController;


    public RaycastHit[] GetHits
    {
        get { return hits; }
    }

    private void Update()
    {
        hits = Physics.BoxCastAll(_transform.position, Vector3.one * 0.5f, _transform.forward, _transform.rotation, detectionRange);

        bool playerPreviouslyInDetectionArea = playerInDetectionArea;
        playerInDetectionArea = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable _damageable))
                {
                    playerInDetectionArea = true;
                    DamageableDetected();
                }
            }
        }

        if (playerPreviouslyInDetectionArea && !playerInDetectionArea)
        {
            DamageableExit();
        }
    }

    private void DamageableDetected()
    {
        if (playerStateController.GetState() == PlayerStates.Idle)
            detected?.Invoke();
    }

    private void DamageableExit()
    {
        leftDetectedAction?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(detectionCubeCentre.position, Vector3.one * detectionRange);
    }
}