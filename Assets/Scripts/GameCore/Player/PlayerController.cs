using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick;

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
    }

}
