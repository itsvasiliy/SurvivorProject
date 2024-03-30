using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyMovement_AccelerationHit : EnemyMovement
{
    [Header("Set for how long the enemy will accelerating")]
    [SerializeField] private float accelerationDuration;

    [Header("Set how often enemy will accelerating")]
    [SerializeField] private float accelerationRate;

    [SerializeField] private float accelerationSpeed;

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

            yield return new WaitForSeconds(accelerationRate);
        }
    }

    private IEnumerator AccelerateEnemy()
    {
        float duration = accelerationDuration;

        while (duration >= 0f)
        {
            Vector3 aimFixedPosition = aimPosition;
            aimFixedPosition.y = enemyTransform.position.y;

            Vector3 direction = (aimPosition - enemyTransform.position).normalized;
            enemyTransform.Translate(direction * accelerationSpeed * Time.fixedDeltaTime);

            duration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}