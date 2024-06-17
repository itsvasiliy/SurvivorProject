using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StructureDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private StructureSpawnManager spawnManager;

    [SerializeField] ScrollRect scrollRect;

    [SerializeField] private Transform structurePreviewGameObject;
    [SerializeField] private GameObject structure;

    [SerializeField] public ResourceController resourceController;

    private Camera mainCamera;

    private bool isDraggingStructure = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraggingStructure = false;

        scrollRect.OnInitializePotentialDrag(eventData);
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggingStructure == true)
        {
            if (Input.touchCount > 0)
            {
                FollowTouch();
            }
            else if (Input.GetMouseButton(0))
            {
                FollowMouse();
            }
        }
        else
        {
            scrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (structurePreviewGameObject.gameObject.activeSelf == true)
        {
            Invoke(nameof(OnEndDragDelay), 0.2f);
        }
    }

    private void OnEndDragDelay() // wait for OnDrop to insure that player didn't return the structure 
    {
        if (structurePreviewGameObject.gameObject.activeSelf == false) return;

        Vector3 spawnPosition = structurePreviewGameObject.position;
        structurePreviewGameObject.gameObject.SetActive(false);

        SpendResources(structure.GetComponent<Structure>());
        spawnManager.SpawnStrucutre(structure, spawnPosition);
    }

    public void OnDrop(PointerEventData eventData) // Player return the item
    {
        structurePreviewGameObject.gameObject.SetActive(false);
    }

    public void ActivateStructureDragging()
    {
        if (IsEnoughResources() == false)
        {
            Debug.Log("Player has no resources");
            return;
        }

        isDraggingStructure = true;

        structurePreviewGameObject.gameObject.SetActive(true);

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(structurePreviewGameObject.position).z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        structurePreviewGameObject.position = new Vector3(worldPosition.x, structurePreviewGameObject.position.y, worldPosition.z);
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(structurePreviewGameObject.position).z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        structurePreviewGameObject.position = new Vector3(worldPosition.x, structurePreviewGameObject.position.y, worldPosition.z);
    }

    private void FollowTouch()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, mainCamera.WorldToScreenPoint(structurePreviewGameObject.position).z);

            Vector3 worldPositionTouch = mainCamera.ScreenToWorldPoint(touchPosition);

            structurePreviewGameObject.position = new Vector3(worldPositionTouch.x, structurePreviewGameObject.position.y, worldPositionTouch.z);
        }
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
        var structureScript = structure.GetComponent<Structure>();
        foreach (var requiredResource in structureScript.constructionCost)
        {
            if (!resourceController.HasEnoughResource(requiredResource.resourceType, requiredResource.cost))
                return false;
        }
        return true;
    }
}
