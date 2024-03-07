using UnityEngine;

public class StructurePlacement : MonoBehaviour
{
    private GameObject structurePrefab;
    private Transform structureViewingTransform;
    private StructPlacementAvailability placementAvailability;

    [SerializeField] GameObject buildButton;


    public void PreviewBuildingPlacement(GameObject gameObject)
    {
        ClearViewer();

        structurePrefab = gameObject;

        GameObject gameObjectCopy = Instantiate(gameObject);

        var viewPosition = new Vector3(transform.position.x, gameObject.transform.position.y + 0.01f, transform.position.z);

        gameObjectCopy.AddComponent<StructPlacementAvailability>();
        var obj = Instantiate(gameObjectCopy, viewPosition, Quaternion.identity, transform);
        Destroy(gameObjectCopy);

        placementAvailability = obj.GetComponent<StructPlacementAvailability>();
        var collider = obj.GetComponent<MeshCollider>();
        collider.isTrigger = true;

        structureViewingTransform = obj.transform;
        buildButton.SetActive(true);
    }

    private void ClearViewer()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }


    public void PlaceStructure()
    {
        if (placementAvailability.canBuild)
        {
            Debug.Log("Pllacing building");

            Instantiate(structurePrefab, structureViewingTransform.position, structureViewingTransform.rotation);
            ClearViewer();
            buildButton.SetActive(false);
        }
        else if (placementAvailability == null)
        {
            Debug.Log("cant buld");
        }
    }
}
