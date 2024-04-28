using System.Collections;
using Assets.Scripts.GameCore.Resources;
using UnityEngine;

public class DropResourcesOnDeath : MonoBehaviour
{
    [SerializeField] ResourceTypes resourceType;

    [SerializeField] int resourceAmount;

    public void DropResources(ResourceController resourceController)
    {
        resourceController.AddResource(resourceType, resourceAmount);
    }
}
