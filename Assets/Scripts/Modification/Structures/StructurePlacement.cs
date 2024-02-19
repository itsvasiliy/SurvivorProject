using UnityEngine;

public class StructurePlacement : MonoBehaviour
{
    public void PreviewBuildingPlacement(GameObject gameObject)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }

        var obj = Instantiate(gameObject, transform.position, Quaternion.identity, transform);
        obj.AddComponent<StructPlacementAvailability>();

        var collider = obj.GetComponent<MeshCollider>();
        collider.isTrigger = true;
    }

    public void PlaceStructure(GameObject gameObject)
    {
        Instantiate(gameObject, transform.position, Quaternion.identity, transform);
    }

}
