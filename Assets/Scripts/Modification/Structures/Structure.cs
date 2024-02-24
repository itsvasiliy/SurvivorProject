using Assets.Scripts.GameCore.Interfaces;
using Assets.Scripts.GameCore.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Structure : MonoBehaviour
{
    public int maxHealth;
    public int health;


    [Serializable]
    public class ResourceCost
    {
        public ResourceTypes resourceType;
        public int cost;
    }

    public List<ResourceCost> constructionCost = new List<ResourceCost>();

    [Inject] IResourceController resourceController;
    [Inject] StructurePlacement structurePlacement;
    [Inject] IPlayerStateController playerStateController;



    public bool IsEnoughResources()
    {
        foreach (var requiredResource in constructionCost)
        {
            if (requiredResource.cost > resourceController.
                GetResourceAmount(requiredResource.resourceType))
                return false;
        }
        return true;
    }

    public void PreviewBuilding()
    {
        playerStateController.SetState(PlayerStates.BuildViewing);
        structurePlacement.PreviewBuildingPlacement(gameObject);
    }


    public void Build()
    {
        playerStateController.SetState(PlayerStates.Idle);
        structurePlacement.PlaceStructure();
    }
}
