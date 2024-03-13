using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcheryShooting : EnemyShooting
{
    [SerializeField] private float arrowSpeed;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            Vector3 spawnOrigin = transform.position;
            spawnOrigin.y += 5f;

            SpawnBulletServerRpc(spawnOrigin);
        }
    }

}
