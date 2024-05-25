using UnityEngine;

public class DragAndDropStructure : MonoBehaviour
{
    new Camera camera;
    private Vector3 offset = new Vector3(2, 2, 2);
    [SerializeField] public Transform structurePreviewPosition;

    public void SetCamera(Camera _camera) => camera = _camera;

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = camera.WorldToScreenPoint(transform.position).z; // Maintain z distance
        return camera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void OnMouseDown()
    {
        // Calculate offset between object position and mouse position
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 newPosition = GetMouseWorldPosition() + offset;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
