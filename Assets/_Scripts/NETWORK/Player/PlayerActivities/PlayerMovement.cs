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

    public override void OnNetworkSpawn()
    {
        // if (!IsOwner) Destroy(this);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Vector3 velocity = movement.normalized * speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + velocity);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            animator.SetBool("IsRunning", true);
            playerStateController.SetState(PlayerStates.Running);
        }
        else
        {
            if (!IsOwner) return;
            animator.SetBool("IsRunning", false);
            if (playerStateController.GetState() != PlayerStates.Shooting)
                playerStateController.SetState(PlayerStates.Idle);
        }
    }

}
