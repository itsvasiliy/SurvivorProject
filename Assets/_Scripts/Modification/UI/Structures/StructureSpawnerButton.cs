using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StructureSpawnerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public ScrollRect scrollRect;
    public StructureDragDrop structureDragDrop;
    private bool isDraggingStructure = false;

    private Vector2 initialPressPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Detect the initial touch position
        initialPressPosition = eventData.pressPosition;
        isDraggingStructure = false;
        structureDragDrop.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDraggingStructure)
        {
            structureDragDrop.OnEndDrag(eventData);
        }
        else
        {
            // Pass the event to the ScrollRect if it's not a drag for structure
            scrollRect.OnEndDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Determine if the drag should be handled as structure drag or scroll
        if (!isDraggingStructure)
        {
            // Calculate the vertical drag distance
            float verticalDragDistance = Mathf.Abs(eventData.position.y - initialPressPosition.y);

            // Consider a threshold to distinguish between a scroll and a structure drag
            if (verticalDragDistance > 50f) // Adjust threshold as needed
            {
                structureDragDrop.ActivateStructureDragging();
                isDraggingStructure = true;
                structureDragDrop.OnDrag(eventData);

            }
            else
            {
                scrollRect.OnDrag(eventData);
            }
        }
        else
        {
            structureDragDrop.OnDrag(eventData);
        }
    }
}