using System;
using UnityEngine;

public class BallisticAmmo : Bullet
{
    private Rigidbody rb;

    private float launchAngle = 25f;


    public override void Launch(Vector3 startPosition, Vector3 target, Action _onBulletCollision)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        else
            rb.linearVelocity = Vector3.zero;

        LaunchInit(startPosition, target, _onBulletCollision);


        Vector3 targetDirection = target - startPosition;
        float distanceToTarget = targetDirection.magnitude;
        float launchAngleRad = launchAngle * Mathf.Deg2Rad;

        float Vx = distanceToTarget / (Mathf.Cos(launchAngleRad) * Mathf.Sqrt((2 * distanceToTarget * Mathf.Sin(launchAngleRad)) / Physics.gravity.magnitude));
        float Vy = Mathf.Sin(launchAngleRad) * Vx;

        Vector3 velocity = Vx * targetDirection.normalized + Vy * Vector3.up;

        rb.linearVelocity = velocity;


        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnCollisionEnter(Collision collision) => Explode(collision.gameObject);

}
