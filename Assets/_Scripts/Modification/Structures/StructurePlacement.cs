using UnityEngine;
using Zenject;

public class StructurePlacement : MonoBehaviour
{
    private GameObject structurePrefab;
    private Transform structureViewingTransform;

    [SerializeField] GameObject buildButton;

    public bool canBuild = true;

    [Inject] DiContainer container;

    public void PreviewBuildingPlacement(GameObject gameObject)
    {
        ClearViewer();

        structurePrefab = gameObject;

        GameObject gameObjectCopy = Instantiate(gameObject);
        gameObjectCopy.AddComponent<StructPlacementAvailability>();
        var obj = container.InstantiatePrefab(gameObjectCopy, transform.position, Quaternion.identity, transform);
        Destroy(gameObjectCopy);

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
        if (canBuild)
        {
            Instantiate(structurePrefab, structureViewingTransform.position, structureViewingTransform.rotation);
            ClearViewer();
            buildButton.SetActive(false);
        }
    }
}
