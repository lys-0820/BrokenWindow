using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Camera mainCamera;
    protected Vector3 targetPosition;
    protected bool isDragging = false;

    [SerializeField] protected float lerpSpeed = 10f; // Lerp speed for smooth dragging

    // Shared variables for boundary checking
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        // Smoothly move the object to the target position
        if (isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Convert mouse position to world position
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Keep the same Z plane

        // Clamp position within screen bounds
        targetPosition = ClampToScreenBounds(mouseWorldPos);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    protected virtual Vector3 ClampToScreenBounds(Vector3 position)
    {
        if (spriteRenderer == null) return position; // Safety check

        float width = spriteRenderer.bounds.extents.x; // Half-width
        float height = spriteRenderer.bounds.extents.y; // Half-height

        // Get screen bounds in world coordinates
        Vector3 minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Adjusted clamping: Ensure edges stay within bounds
        position.x = Mathf.Clamp(position.x, minScreenBounds.x + width, maxScreenBounds.x - width);
        position.y = Mathf.Clamp(position.y, minScreenBounds.y + height, maxScreenBounds.y - height);

        return position;
    }
}