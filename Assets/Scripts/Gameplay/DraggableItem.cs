using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Camera mainCamera;
    protected Vector3 targetPosition;
    protected bool isDragging = false;
    private Vector3 originalPosition;

    [SerializeField] protected float lerpSpeed = 10f; // Lerp speed for smooth dragging
    protected SpriteRenderer spriteRenderer;

    public void Init(Camera camera)
    {
        mainCamera = camera;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null)
        {
            Debug.LogError("DraggableItem: Main camera not assigned!");
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("DraggableItem: SpriteRenderer component is missing!");
        }
    }

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
    }

    protected virtual void Update()
    {
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
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        targetPosition = ClampToScreenBounds(mouseWorldPos);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // Check for the nearest drop zone
        Collider2D dropZone = GetNearestDropZone();
        if (dropZone != null)
        {
            transform.position = dropZone.transform.position; // Snap to drop zone
        }
        else
        {
            transform.position = originalPosition; // Return to original position if no valid drop zone
        }
    }

    private Collider2D GetNearestDropZone()
    {
        List<Collider2D> dropZones = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, 0.5f));

        foreach (Collider2D col in dropZones)
        {
            if (col.CompareTag("DropZone"))
            {
                return col; // Return the first valid drop zone found
            }
        }
        return null; // No valid drop zone found
    }

    protected virtual Vector3 ClampToScreenBounds(Vector3 position)
    {
        if (spriteRenderer == null) return position;

        float width = spriteRenderer.bounds.extents.x;
        float height = spriteRenderer.bounds.extents.y;

        // Get screen bounds in world coordinates
        Vector3 minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Adjusted clamping: Ensure edges stay within bounds
        position.x = Mathf.Clamp(position.x, minScreenBounds.x + width, maxScreenBounds.x - width);
        position.y = Mathf.Clamp(position.y, minScreenBounds.y + height, maxScreenBounds.y - height);

        return position;
    }
}
