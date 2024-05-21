using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnArea;
    public NetworkObject[] PrefabsToSpawn;
    public int SpawnCount = 1;
    public float SpawnInterval = 1f;
    private NetworkObject[] m_SpawnedNetworkObjects;

    private bool canSpawn = true;
    public void StopSpawning()
    {
        canSpawn = false;
        StopCoroutine(StartSpawningCoroutine());
    }
    public void StartSpawning()
    {
        canSpawn = true;
        StartCoroutine(StartSpawningCoroutine());
    }



    private IEnumerator StartSpawningCoroutine()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            if (!canSpawn)
                yield break;

            SpawnWithServerRpc();
            yield return new WaitForSeconds(SpawnInterval);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnWithServerRpc()
    {
        var randomPrefab = PrefabsToSpawn[Random.Range(0, PrefabsToSpawn.Length)];
        NetworkObject no = Instantiate(randomPrefab, GetRandomPositionInSpawnArea(), Quaternion.identity);
        no.Spawn();
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

}