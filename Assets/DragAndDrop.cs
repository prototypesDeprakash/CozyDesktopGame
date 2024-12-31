using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Rigidbody rb;
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;

    private void Start()
    {
       
        mainCamera = Camera.main;

       
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse down on the object.");
        if (rb != null)
        {
            isDragging = true;
            rb.isKinematic = true; // Temporarily disable physics while dragging
            offset = transform.position - GetMouseWorldPosition();
        }
    }

    private void OnMouseDrag()
    {
        Debug.Log("Dragging the object.");
        if (isDragging && rb != null)
        {
            Vector3 newPos = GetMouseWorldPosition() + offset;
            rb.MovePosition(newPos); // Move the Rigidbody smoothly
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse released.");
        if (rb != null)
        {
            isDragging = false;
            rb.isKinematic = false; // Re-enable physics
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}
