using Assets.Scripts.GameCore.Resources;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class StructureUISelection : MonoBehaviour
{
    [SerializeField] NetworkObject structure;
    
    [SerializeField] TextMeshProUGUI woodPrice;
    [SerializeField] TextMeshProUGUI stonePrice;

    PlayerStateController playerStateController;

    StructurePlacement structurePlacement;

    private void Start()
    {
        var cost = structure.GetComponent<Structure>().constructionCost;

        var wood = cost.Where(x => x.resourceType == ResourceTypes.WOOD).FirstOrDefault();
        if (wood != null)
            woodPrice.text = wood.cost.ToString();

        var stone = cost.Where(x => x.resourceType == ResourceTypes.STONE).FirstOrDefault();
        if (stone != null)
            stonePrice.text = stone.cost.ToString();
    }

    public void SetStructurePlacement(StructurePlacement _structurePlacement) => structurePlacement = _structurePlacement;
    public void SetPlayerStateController(PlayerStateController _playerStateController) => playerStateController = _playerStateController;

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

        playerStateController.SetState(PlayerStates.BuildViewing);
        structurePlacement.PreviewBuildingPlacement(structure);
    }
}
