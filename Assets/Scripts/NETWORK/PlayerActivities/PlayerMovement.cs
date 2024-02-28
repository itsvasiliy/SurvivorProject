using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private FloatingJoystick joystick;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] Animator animator;

    [SerializeField] float speed;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(joystick.Horizontal * speed, _rigidbody.angularVelocity.y * speed, joystick.Vertical * speed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            playerTransform.rotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
            animator.SetBool("IsRunning", true);

            //   playerStateController.SetState(PlayerStates.Running);
        }
        else
        {
            animator.SetBool("IsRunning", false);

            //  playerStateController.SetState(PlayerStates.Idle); //wrong behavior
        }
    }
}
