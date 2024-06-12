using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StructureSpawnManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> structurePrefabs;

    public void SpawnStrucutre(GameObject structure, Vector3 spawnPosition)
    {
        int prefabIndex = structurePrefabs.IndexOf(structure);

        if (prefabIndex == -1)
        {
            Debug.LogError("Structure prefab not found in the list!");
            return;
        }

        SpawnStructureServerRpc(prefabIndex, spawnPosition);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnStructureServerRpc(int prefabIndex, Vector3 spawnPosition)
    {
        SpawnStructureClientRpc(prefabIndex, spawnPosition);
    }

    [ClientRpc]
    private void SpawnStructureClientRpc(int prefabIndex, Vector3 spawnPosition)
    {
        if (prefabIndex < 0 || prefabIndex >= structurePrefabs.Count)
        {
            Debug.LogError("Invalid prefab index!");
            return;
        }

        Instantiate(structurePrefabs[prefabIndex], spawnPosition, structurePrefabs[prefabIndex].transform.rotation);
    }
}
