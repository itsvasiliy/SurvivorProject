using System.Collections.Generic;
using UnityEngine;

public class StructureShopInitialization : MonoBehaviour
{
    [SerializeField] PlayerStateController playerStateController;

    [SerializeField] StructurePlacement structurePlacement;

    [SerializeField] List<StructureUISelection> structureUISelections = new List<StructureUISelection>();

    void Start()
    {
        foreach (StructureUISelection selection in structureUISelections)
        {
            selection.SetPlayerStateController(playerStateController);
            selection.SetStructurePlacement(structurePlacement);
        }
    }
}
