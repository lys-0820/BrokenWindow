using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Camera mainCamera;
    protected Vector3 targetPosition;
    protected bool isDragging = false;
    private Vector3 originalPosition;
    private PlantDropZone originalDropZone;
    private PlantDropZone[] allDropZones;

    public bool justSpawned = false;
    public PlantType plantType = PlantType.Potted;

    [SerializeField] protected float lerpSpeed = 10f; // Lerp speed for smooth dragging
    protected SpriteRenderer spriteRenderer;

    public void Init(Camera camera)
    {
        mainCamera = camera;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        allDropZones = FindObjectsByType<PlantDropZone>(FindObjectsSortMode.None);
    }

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        originalPosition = transform.position;
        isDragging = true;
        ShowValidDropZones();

        // Store the original drop zone before dragging
        Collider2D dropZoneCollider = GetNearestDropZone();
        if (dropZoneCollider != null)
        {
            originalDropZone = dropZoneCollider.GetComponent<PlantDropZone>();
        }

        if (originalDropZone != null && originalDropZone.placedPlant == gameObject)
        {
            originalDropZone.placedPlant = null; // Remove plant from the old drop zone
        }
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
        HideAllDropZones();

        // Check for the nearest drop zone
        Collider2D dropZoneCollider = GetNearestDropZone();
        PlantDropZone plantDropZone =
            dropZoneCollider ? dropZoneCollider.GetComponent<PlantDropZone>() : null;
        if (plantDropZone != null
                && plantDropZone.placedPlant == null
                && plantDropZone.plantType == plantType)
        {
            plantDropZone.placedPlant = gameObject;
            transform.position = dropZoneCollider.transform.position; // Snap to drop zone
            justSpawned = false;
        }
        else
        {
            HandleIllegalPlantDrop();
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

    public void OnDropSuccessful()
    {
        isDragging = false;
    }

    private void HandleIllegalPlantDrop()
    {
        if (justSpawned)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = originalPosition;
            if (originalDropZone != null && originalDropZone.placedPlant == null)
            {
                originalDropZone.placedPlant = gameObject;
            }
        }
    }

    private void ShowValidDropZones() {
        foreach (var dropZone in allDropZones)
        {
            if (dropZone.plantType == plantType)
            {
                dropZone.ShowDropZone();
            }
        }
    }

    private void HideAllDropZones()
    {
        foreach (var dropZone in allDropZones)
        {
            dropZone.HideDropZone();
        }
    }
}
