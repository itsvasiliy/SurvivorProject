using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int totalEnemiesToSpawn = 12;
    public int enemiesPerGroup = 3;
    public float spawnInterval = 1f;
    public float spawnRadius = 5f;

    private int enemiesSpawned = 0;
    private int enemiesInCurrentGroup = 0;
    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (enemiesSpawned < totalEnemiesToSpawn)
        {
            if (Time.time >= nextSpawnTime)
            {
                if (enemiesInCurrentGroup < enemiesPerGroup)
                {
                    Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                    spawnPosition.y = 0.5f;
                    GameObject newEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                    newEnemy.transform.parent = transform; 
                    enemiesSpawned++;
                    enemiesInCurrentGroup++;
                    nextSpawnTime = Time.time + spawnInterval;
                }
                else
                {
                    enemiesInCurrentGroup = 0;
                    nextSpawnTime = Time.time + spawnInterval * 3;
                }
            }
        }
        else
        {
            
            int remainingEnemies = transform.childCount - totalEnemiesToSpawn;
            if (remainingEnemies < 0)
            {
                totalEnemiesToSpawn += Mathf.Abs(remainingEnemies);
            }
        }
    }
}
