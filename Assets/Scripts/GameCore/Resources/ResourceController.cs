using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ResourceController : MonoBehaviour, IResourceController
{
    [SerializeField] ResourcesUIControl resourcesUIControl;

    private Dictionary<ResourceTypes, int> resources = new Dictionary<ResourceTypes, int>();


    public void AddResource(ResourceTypes type, int amount)
    {
        if (resources.ContainsKey(type))
            resources[type] += amount;
        else
            resources[type] = amount;

       resourcesUIControl.UpdateResource(type);
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
        {
            resources[type] -= amount;
            resourcesUIControl.UpdateResource(type);
        }
        else
            throw new KeyNotFoundException($"The resource type '{type}' does not exist in the dictionary to remove it.");
    }

}
