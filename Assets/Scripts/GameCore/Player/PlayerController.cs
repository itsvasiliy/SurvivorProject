using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] Animator animator;

    [SerializeField] float speed;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(joystick.Horizontal * speed, rb.angularVelocity.y * speed,
            joystick.Vertical * speed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
            animator.SetBool("IsRunning", true);
        }
        else
            animator.SetBool("IsRunning", false);
    }

}
