using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StructureSpawnManager : NetworkBehaviour
{
    private GameObject structure;

    public void SpawnStrucutre(GameObject structure, Vector3 spawnPosition)
    {
        this.structure = structure;
        SpawnStrucutreServerRpc(spawnPosition);
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnStrucutreServerRpc(Vector3 spawnPosition)
    {
        SpawnStrucutreClientRpc(spawnPosition);
    }

    [ClientRpc]
    private void SpawnStrucutreClientRpc(Vector3 spawnPosition)
    {
        Instantiate(structure, spawnPosition, Quaternion.identity);
    }
}
