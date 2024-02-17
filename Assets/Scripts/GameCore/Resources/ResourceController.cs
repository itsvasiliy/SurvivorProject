using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour, IResourceController
{
    Dictionary<ResourceTypes, int> resources;

    private void Start()
    {
        resources = new Dictionary<ResourceTypes, int>();
    }

    public void AddResource(ResourceTypes type, int amount)
    {
        if (resources.ContainsKey(type))
            resources[type] += amount;
        else
            resources[type] = amount;
    }

    public int GetResourceAmount(ResourceTypes type)
    {
        if (resources.ContainsKey(type))
            return resources[type];
        return 0;
    }

    public bool HasEnoughResource(ResourceTypes type, int amount)
    {
        if (resources.ContainsKey(type))
            return resources[type] >= amount;
        return false;
    }

    public void RemoveResource(ResourceTypes type, int amount)
    {
        if (resources.ContainsKey(type))
            resources[type] -= amount;
        else
            throw new KeyNotFoundException($"The resource type '{type}' does not exist in the dictionary to remove it.");
    }

}
