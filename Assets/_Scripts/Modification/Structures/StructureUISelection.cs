using Unity.Netcode;
using UnityEngine;

public class StructureUISelection : MonoBehaviour
{
    [SerializeField] NetworkObject structure;
    [SerializeField] StructurePlacement structurePlacement;


    public bool IsEnoughResources()
    {
        var structureScript = structure.GetComponent<Structure>();
        foreach (var requiredResource in structureScript.constructionCost)
        {
            if (!structurePlacement.resourceController.HasEnoughResource(requiredResource.resourceType, requiredResource.cost))
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
