using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class StructurePrefabFactory : MonoBehaviour
{
    public List<NetworkObject> structureDictionary = new List<NetworkObject>();


    public NetworkObject GetNetworkPrefab(string key)
    {
        foreach (var entry in structureDictionary)
        {
            var structName = entry.GetComponent<Structure>();
            if (structName.structureName == key)
                return entry;
        }

        Debug.LogWarning("Prefab with key " + key + " is not registered.");
        return null;
    }
}
