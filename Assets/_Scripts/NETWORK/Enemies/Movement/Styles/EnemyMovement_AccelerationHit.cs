using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement_AccelerationHit : EnemyMovement
{
    [Header("Set the attack rate in seconds")]
    [SerializeField] private float attackRate;

    [Header("Set for how long will the enemy accelerate in the plyaer direction")]
    [SerializeField] private float accelerationDuration;

    private Vector3 aimPosition;

    private void Start()
    {
        StartCoroutine(CheckPlayersAround());
    }

    private IEnumerator CheckPlayersAround()
    {
        while (true)
        {
            Transform closestPlayer = GetClosestPlayer();

            if (closestPlayer != null)
            {
                aimPosition = closestPlayer.transform.position;
                StartCoroutine(AccelerateEnemy());
            }

            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator AccelerateEnemy()
    {
        float duration = accelerationDuration;

        while (duration >= 0f)
        {
            Vector3 direction = (aimPosition - enemyTransform.position).normalized;
            enemyTransform.Translate(direction * moveSpeed * Time.fixedDeltaTime);

            duration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}