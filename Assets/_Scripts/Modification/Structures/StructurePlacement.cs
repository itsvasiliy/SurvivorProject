using Unity.Netcode;
using UnityEngine;

public class StructurePlacementParams : INetworkSerializable
{
    public string structureName;
    public Vector3 position;
    public Quaternion rotation;
    public bool canBuild;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref structureName);
        serializer.SerializeValue(ref position);
        serializer.SerializeValue(ref rotation);
        serializer.SerializeValue(ref canBuild);
    }
}


public class StructurePlacement : NetworkBehaviour
{
    [SerializeField] private GameObject buildButton;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private ResourceController resourceController;

    private NetworkObject structurePrefab;

    private StructurePrefabFactory structurePrefabFactory;

    private Transform viewingStructureTransform;

    private StructPlacementAvailability placementAvailability;



    private void Start() => structurePrefabFactory = FindFirstObjectByType<StructurePrefabFactory>();

    public void PreviewBuildingPlacement(NetworkObject netStructureOrigin)
    {
        ClearViewer();

        var viewPosition = new Vector3(transform.position.x, netStructureOrigin.transform.position.y + 0.04f, transform.position.z);

        var obj = Instantiate(netStructureOrigin.gameObject, viewPosition, Quaternion.identity);
        obj.transform.localRotation = Quaternion.identity;

        obj.GetComponent<Structure>().isViewing = true;

        obj.AddComponent<StructPlacementAvailability>();
        var follower = obj.AddComponent<StructurePlayerFrontFollower>();
        follower.playerTransform = resourceController.transform;
        follower.structure = obj.GetComponent<Structure>();

        var colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
            collider.isTrigger = true;

        SetButtonStatus(true);

        structurePrefab = netStructureOrigin;
        viewingStructureTransform = obj.transform;
        placementAvailability = obj.GetComponent<StructPlacementAvailability>();
    }

    private void ClearViewer()
    {
        if (viewingStructureTransform != null)
            Destroy(viewingStructureTransform.gameObject);
    }


    private void SpendResources(Structure structureScript)
    {
        foreach (var requiredResource in structureScript.constructionCost)
        {
            resourceController.RemoveResource(requiredResource.resourceType,
                requiredResource.cost);
        }
    }

    public bool IsEnoughResources()
    {
        var structureScript = structurePrefab.GetComponent<Structure>();
        foreach (var requiredResource in structureScript.constructionCost)
        {
            if (!resourceController.HasEnoughResource(requiredResource.resourceType, requiredResource.cost))
                return false;
        }
        return true;
    }


    public void PlaceStructure()
    {
        if (placementAvailability.canBuild == false)
            return;

        var structureScript = structurePrefab.GetComponent<Structure>();
        var _structParams = new StructurePlacementParams()
        {
            position = viewingStructureTransform.position,
            rotation = viewingStructureTransform.rotation,
            structureName = structureScript.structureName,
        };

        SpendResources(structureScript);
        PlaceStructureServerRpc(_structParams);

        if (IsEnoughResources() == false)
            CancelPlacing();
        else
            viewingStructureTransform.GetComponent<Structure>().canFollow = true;
    }


    [ServerRpc(RequireOwnership = false)]
    public void PlaceStructureServerRpc(StructurePlacementParams _params)
    {
        var _structurePrefab = structurePrefabFactory.GetNetworkPrefab(_params.structureName);

        NetworkObject netStructure = Instantiate(_structurePrefab, _params.position,
            _params.rotation);
        netStructure.Spawn();
    }

    public void CancelPlacing()
    {
        ClearViewer();
        SetButtonStatus(false);
    }

    private void SetButtonStatus(bool status)
    {
        buildButton.SetActive(status);
        cancelButton.SetActive(status);
    }
}

