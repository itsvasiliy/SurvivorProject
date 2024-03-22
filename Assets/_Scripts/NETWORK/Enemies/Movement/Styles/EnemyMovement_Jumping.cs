using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement_Jumping : EnemyMovement
{
    [SerializeField] private AnimationCurve jumpCurve;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float jumpRate;

    private Vector3 initialPosition;
    private Vector3 landingPosition;

    private float jumpTime;

    private void Start()
    {
        Invoke(nameof(UpdateLandingPosition), jumpRate);
    }

    private Vector3 GetLandingPosition() // not done yet
    {
        Vector3 landingDirection = Vector3.zero;
        Vector3 calculatedLandingPosition = Vector3.zero;

        float randomDegreeAngle = 0;

        switch (Random.Range(0, 4))
        {
            case 0: // Forward

                randomDegreeAngle = Random.Range(-30f, 30f);
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                landingDirection = Vector3.forward;

                break;
            case 1: // Backward

                randomDegreeAngle = Random.Range(150f, 210f);
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                landingDirection = Vector3.back;

                break;
            case 2: // Left

                randomDegreeAngle = Random.Range(60f, 120f);
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                landingDirection = Vector3.left;

                break;
            case 3: // Right

                randomDegreeAngle = Random.Range(-120f, -60f);
                enemyTransform.rotation = Quaternion.Euler(0f, randomDegreeAngle, 0f);
                landingDirection = Vector3.right;

                break;
            default:
                break;
        }

        return calculatedLandingPosition;
    }

    private void UpdateLandingPosition()
    {
        landingPosition = GetLandingPosition();
    }

    private void Update()
    {
        if (jumpTime <= 1f)
        {
            // Calculate the jump height using the curve
            float height = jumpCurve.Evaluate(jumpTime) * jumpHeight;

            Vector3 targetPosition = Vector3.Lerp(initialPosition, landingPosition, jumpTime) + Vector3.up * height;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, accelerationSpeed * Time.deltaTime);

            jumpTime += Time.fixedDeltaTime;
        }
        else
        {
            // Reset
            jumpTime = 0f;
            initialPosition = transform.position;
        }
    }


}