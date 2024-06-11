using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureDragDrop : MonoBehaviour
{
    public void ShowStructurePreview(StructureInfo structure)
    {
        structure.ActivateStructurePreviewGameObject();
    }

    public void HideTheStructure(StructureInfo structure)
    {
        structure.DeactivateStructurePreviewGameObject();
    }
}
