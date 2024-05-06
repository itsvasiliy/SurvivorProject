using UnityEngine;

public class StructurePlayerFrontFollower : MonoBehaviour
{
    public Transform playerTransform;

    public Structure structure;

    public float distance = 3f; 

    void Start()
    {
        if (playerTransform == null || structure == null)
            Debug.LogWarning("Player reference or structure are not set in PlayerFollower script.");
    }


    private void Update()
    {
        if (structure.canFollow)
        {
            Vector3 targetPosition = playerTransform.position + playerTransform.forward * distance;
            transform.position = targetPosition;
        }
    }
}
