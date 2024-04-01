using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class SpawnEnemy : NetworkBehaviour
{
    [SerializeField] private GameObject spawnArea; 
    public GameObject[] PrefabsToSpawn; 
    public bool DestroyWithSpawner;
    public int SpawnCount = 1;
    public float SpawnInterval = 1f;
    private NetworkObject[] m_SpawnedNetworkObjects;

    private IEnumerator StartSpawningCoroutine()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInSpawnArea(); 
            Quaternion spawnRotation = Quaternion.identity;

            GameObject prefab = PrefabsToSpawn[Random.Range(0, PrefabsToSpawn.Length)]; 
            GameObject spawnedObject = Instantiate(prefab, spawnPosition, spawnRotation);
            m_SpawnedNetworkObjects[i] = spawnedObject.GetComponent<NetworkObject>();
            m_SpawnedNetworkObjects[i].Spawn();

            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    private Vector3 GetRandomPositionInSpawnArea()
    {
     
        if (spawnArea == null || spawnArea.GetComponent<Collider>() == null)
        {
            Debug.LogError("Spawn area or its collider is not assigned.");
            return Vector3.zero;
        }

      
        Bounds bounds = spawnArea.GetComponent<Collider>().bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

      
        return new Vector3(randomX, randomY, randomZ);
    }

    public override void OnNetworkSpawn()
    {
        enabled = IsServer;
        if (!enabled || PrefabsToSpawn.Length == 0 || spawnArea == null)
        {
            return;
        }

        m_SpawnedNetworkObjects = new NetworkObject[SpawnCount]; 

        StartCoroutine(StartSpawningCoroutine());
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkObjects != null)
        {
            for (int i = 0; i < SpawnCount; i++)
            {
                if (m_SpawnedNetworkObjects[i] != null && m_SpawnedNetworkObjects[i].IsSpawned)
                {
                    m_SpawnedNetworkObjects[i].Despawn();
                }
            }
        }
        base.OnNetworkDespawn();
    }
}
