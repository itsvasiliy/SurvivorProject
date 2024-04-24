using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float minSpawnTime = 1f; 
    public float maxSpawnTime = 5f; 

    private float nextSpawnTime;
    private GameObject previousSpawnedObject; 

    private void Start()
    {

        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void Update()
    {
 
        if (Time.time >= nextSpawnTime)
        {
            
            float randomX = Random.Range(-100f, 100f);
            float randomZ = Random.Range(-100f, 100f);

            GameObject newObject = Instantiate(objectToSpawn, new Vector3(randomX, objectToSpawn.transform.position.y, randomZ), Quaternion.identity);

           
            if (previousSpawnedObject != null)
            {
                Destroy(previousSpawnedObject);
            }

            
            previousSpawnedObject = newObject;

            
            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
}
