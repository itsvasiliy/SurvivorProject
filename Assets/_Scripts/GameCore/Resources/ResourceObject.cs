using Assets.Scripts.GameCore.Resources;
using UnityEngine;

public class ResourceObject : MonoBehaviour, IMineable
{
    [SerializeField] private ResourceTypes resourceType;
    [SerializeField] private int resourceDropAmount;


    public void MineResource(ResourceController playerResourceController) =>
        playerResourceController.AddResource(resourceType, resourceDropAmount);
}
