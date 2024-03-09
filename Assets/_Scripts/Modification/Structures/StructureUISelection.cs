using Unity.Netcode;
using UnityEngine;

public class StructureUISelection : NetworkBehaviour
{
    [SerializeField] NetworkObject structure;
    [SerializeField] StructurePlacement structurePlacement;
    [SerializeField] ResourceController resourceController;


    public bool IsEnoughResources()
    {
        var structureScript = structure.GetComponent<Structure>();
        foreach (var requiredResource in structureScript.constructionCost)
        {
            if (requiredResource.cost > resourceController.
                GetResourceAmount(requiredResource.resourceType))
                return false;
        }
        return true;
    }


    public void PreviewBuilding()
    {
        //  playerStateController.SetState(PlayerStates.BuildViewing);
        structurePlacement.PreviewBuildingPlacement(structure);
    }
}
