using Unity.Netcode;
using UnityEngine;

public class StructureUISelection : MonoBehaviour
{
    [SerializeField] NetworkObject structure;
    [SerializeField] StructurePlacement structurePlacement;
    [SerializeField] ResourceController resourceController;


    public bool IsEnoughResources()
    {
        var structureScript = structure.GetComponent<Structure>();
        foreach (var requiredResource in structureScript.constructionCost)
        {
            if (!resourceController.HasEnoughResource(requiredResource.resourceType, requiredResource.cost))
                return false;
        }
        return true;
    }


    public void PreviewBuilding()
    {
        if (IsEnoughResources() == false)
        {
            Debug.Log("Not enough resources");
            return;
        }

        //  playerStateController.SetState(PlayerStates.BuildViewing);
        structurePlacement.PreviewBuildingPlacement(structure);
    }
}
