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
    [Header("UI Elements")]
    [SerializeField] private GameObject buildButtons;
    [SerializeField] private GameObject joystick;

    [Header("Drag And Drop Components")]
    [SerializeField] private GameObject dragAndDropController;
    [SerializeField] private Transform structureSpawnTransform;
    [SerializeField] private new Camera camera;

    [Header("Player dependencies")]
    [SerializeField] public ResourceController resourceController;
    [SerializeField] private PlayerStateController playerStateController;


    private NetworkObject structurePrefab;

    private StructurePrefabFactory structurePrefabFactory;

    private Transform viewingStructureTransform;

    private GameObject dndController;

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

        if (dndController == null)
        {
            dndController = Instantiate(dragAndDropController, structureSpawnTransform.position, dragAndDropController.transform.rotation);
            dndController.GetComponent<DragAndDropStructure>().SetCamera(camera);
        }
        else
        {
            ResetViewPosition();
            dndController.SetActive(true);
        }

        var obj = Instantiate(netStructureOrigin.gameObject, Vector3.zero, Quaternion.identity, dndController.transform);
        obj.transform.localPosition = Vector3.zero;

        structurePrefab = netStructureOrigin;
        viewingStructureTransform = obj.transform;
        placementAvailability = obj.AddComponent<StructPlacementAvailability>();
        obj.GetComponent<Structure>().isViewing = true;

        SetButtonStatus(true);
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
        dndController.SetActive(false);
    }

    private void SetButtonStatus(bool status)
    {
        buildButtons.SetActive(status);

        joystick.SetActive(!status);
    }

    private void ResetViewPosition() => dndController.transform.position = structureSpawnTransform.position;
}

