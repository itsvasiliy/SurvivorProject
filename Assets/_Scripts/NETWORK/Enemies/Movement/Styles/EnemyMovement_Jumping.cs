using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement_Jumping : EnemyMovement
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpRate;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Invoke(nameof(Jumping), jumpRate);
    }

    private void Jumping()
    {
        float randomDegreeAngle = 0;

        switch (Random.Range(0, 4))
        {
            case 0: // Forward

                randomDegreeAngle = 0f;
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                JumpForward();

                break;
            case 1: // Backward

                randomDegreeAngle = 180f;
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                JumpForward();

                break;
            case 2: // Left

                randomDegreeAngle = -90f;
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                JumpForward();

                break;
            case 3: // Right

                randomDegreeAngle = 90f;
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                JumpForward();

                break;
            default:
                break;
        }

        Invoke(nameof(Jumping), jumpRate);
    }

    private void JumpForward()
    {
        Vector3 jumpDirection = base.enemyTransform.forward * jumpForce;

        _rigidbody.AddForce(jumpDirection, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}