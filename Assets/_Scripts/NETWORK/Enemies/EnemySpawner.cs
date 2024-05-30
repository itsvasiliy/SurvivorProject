using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnArea;
    [SerializeField] private NetworkObject[] prefabsToSpawn;

    [SerializeField] private int poolCount;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnInterval;

    private ObjectPool<NetworkObject> enemiesPool;

    private bool canSpawn = true;



    private void Awake() => enemiesPool = new ObjectPool<NetworkObject>(Preload, GetAction, ReturnAction, poolCount);

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
        for (int i = 0; i < spawnCount; i++)
        {
            if (!canSpawn)
                yield break;

            var enemy = enemiesPool.Get();
            if (enemy.IsSpawned == false)
                enemy.Spawn();

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private NetworkObject CreateEnemy()
    {
        var randomPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
        NetworkObject no = Instantiate(randomPrefab, Vector3.zero, Quaternion.identity);
        return no;
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
        if (!enabled || prefabsToSpawn.Length == 0 || spawnArea == null)
            return;

        StartCoroutine(StartSpawningCoroutine());
    }


    public NetworkObject Preload() => CreateEnemy();

    public void GetAction(NetworkObject enemy)
    {
        var enemyhealth = enemy.GetComponent<EnemyHealthController>();

        enemyhealth.SetReturnAction(OnDeathCase);
        void OnDeathCase() => enemiesPool.Return(enemy);

        enemy.transform.position = GetRandomPositionInSpawnArea();
        if (enemyhealth.IsAlive())
            enemy.gameObject.SetActive(true);
        else
            enemyhealth.Respawn();
    }

    public void ReturnAction(NetworkObject enemy) => enemy.gameObject.SetActive(false);
}