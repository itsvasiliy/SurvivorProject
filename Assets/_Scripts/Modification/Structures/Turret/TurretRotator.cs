using UnityEngine;

public class TurretRotator : MonoBehaviour
{
    [SerializeField] private ShootingTracking shootingTracking;
    [SerializeField] float rotationSpeed = 500f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => shootingTracking.rotatorDelegate += RotateToTarget;

    private void OnDisable() => shootingTracking.rotatorDelegate -= RotateToTarget;

    private void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 relativePosition = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        // Create a rotation quaternion that removes the X-axis rotation
        Quaternion targetRotationWithoutX = Quaternion.Euler(0f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotationWithoutX, rotationSpeed * Time.fixedDeltaTime));
    }

}
