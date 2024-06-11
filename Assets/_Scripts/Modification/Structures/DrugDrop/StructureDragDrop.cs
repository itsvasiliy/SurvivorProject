using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StructureDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler,IEndDragHandler, IDropHandler
{
    [SerializeField] private StructureSpawnManager spawnManager;

    [SerializeField] private Transform structurePreviewGameObject;
    [SerializeField] private GameObject structure;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        structurePreviewGameObject.gameObject.SetActive(true);

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(structurePreviewGameObject.position).z);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        structurePreviewGameObject.position = new Vector3(worldPosition.x, structurePreviewGameObject.position.y, worldPosition.z);
    }

    public void OnDrag(PointerEventData eventData)
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

    public void OnEndDrag(PointerEventData eventData) 
    {
        if(structurePreviewGameObject.gameObject.activeSelf == true)
        {
            Vector3 spawnPosition = structurePreviewGameObject.position;
            structurePreviewGameObject.gameObject.SetActive(false);

            spawnManager.SpawnStrucutre(structure, spawnPosition);
        }
    }

    public void OnDrop(PointerEventData eventData) // Player return the item
    {
        structurePreviewGameObject.gameObject.SetActive(false);
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

}
