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
    [SerializeField] private GameObject buildButtons;
    [SerializeField] private GameObject joystick;

    [SerializeField] private Transform structureSpawnTransform;

    [SerializeField] private new Camera camera;

    [SerializeField] public ResourceController resourceController;

    [SerializeField] private PlayerStateController playerStateController;


    private NetworkObject structurePrefab;

    private StructurePrefabFactory structurePrefabFactory;

    private Transform viewingStructureTransform;

    private StructPlacementAvailability placementAvailability;



    private void Start()
    {
        structurePrefabFactory = FindFirstObjectByType<StructurePrefabFactory>();
        if (structurePrefabFactory == null)
            Debug.LogWarning("Add StructurePrefabFactory to the scene to be able place structures");
    }

    public void PreviewBuildingPlacement(NetworkObject netStructureOrigin)
    {
        ClearViewer();

        var viewPosition = new Vector3(structureSpawnTransform.position.x,
            netStructureOrigin.transform.position.y + 0.04f, structureSpawnTransform.transform.position.z);

        var obj = Instantiate(netStructureOrigin.gameObject, viewPosition, Quaternion.identity);
        obj.transform.localRotation = netStructureOrigin.transform.rotation;

        obj.GetComponent<Structure>().isViewing = true;

        obj.AddComponent<StructPlacementAvailability>();
        var dragNdrop = obj.AddComponent<DragAndDropStructure>();
        dragNdrop.SetCamera(camera);

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

        if (structurePrefabFactory == null)
        {
            Debug.LogError("Add StructurePrefabFactory to the scene to be able place structures");
            return;
        }

        var structureScript = structurePrefab.GetComponent<Structure>();
        var _structParams = new StructurePlacementParams()
        {
            position = viewingStructureTransform.position,
            rotation = viewingStructureTransform.rotation,
            structureName = structureScript.structureName,
        };

        SpendResources(structureScript);
        PlaceStructureServerRpc(_structParams);

        if (IsEnoughResources() && structureScript.name == "Fence") //continue building in case sctruct is fence
            ResetViewPosition();
        else
            CancelPlacing();
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
        playerStateController.SetState(PlayerStates.Idle);
    }

    private void SetButtonStatus(bool status)
    {
        buildButtons.SetActive(status);

        joystick.SetActive(!status);
    }

    private void ResetViewPosition()
    {
        var viewPosition = new Vector3(structureSpawnTransform.position.x,
         viewingStructureTransform.transform.position.y + 0.04f, structureSpawnTransform.transform.position.z);
        viewingStructureTransform.position = viewPosition;
    }
}

