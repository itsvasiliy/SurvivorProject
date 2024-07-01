using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private PlayerStateController playerStateController;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] Animator animator;

    [SerializeField] float speed;

    [SerializeField] float rotationSpeed;

    [SerializeField] GameObject footStepsSound;


    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        float movementMagnitude = movement.magnitude;

        Vector3 scaledMovement = movement.normalized * Mathf.Lerp(0f, speed, movementMagnitude);
        _rigidbody.linearVelocity = scaledMovement;

        if (movementMagnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            animator.SetBool("IsRunning", true);
            playerStateController.TryToSetRunningState();
            footStepsSound.SetActive(true);
        }
        else
        {
            if (!IsOwner)
                return;

            _rigidbody.linearVelocity = Vector3.zero;
            animator.SetBool("IsRunning", false);
            playerStateController.TryToSetIdleState();
            footStepsSound.SetActive(false);
        }
    }

    private void OnDisable() => footStepsSound.SetActive(false);
}
