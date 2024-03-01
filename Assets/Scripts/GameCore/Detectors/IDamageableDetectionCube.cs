using System;
using UnityEngine;

public class IDamageableDetectionCube : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Transform detectionCubeCentre;

    [Range(0, 10)] public float detectionRange;

    [SerializeField] PlayerStateController playerStateController;

    public Color gizmoColor = Color.red;

    public event Action detected;

    public event Action leftDetectedAction;

    private RaycastHit[] hits;

    private bool playerInDetectionArea = false;

    private bool playerPreviouslyInDetectionArea;

    public RaycastHit[] GetHits
    {
        get { return hits; }
    }

    private void Update()
    {
        hits = Physics.BoxCastAll(playerTransform.position, Vector3.one * 0.5f, playerTransform.forward, playerTransform.rotation, detectionRange);

        playerPreviouslyInDetectionArea = playerInDetectionArea;
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