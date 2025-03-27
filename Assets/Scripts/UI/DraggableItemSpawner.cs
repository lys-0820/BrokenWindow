using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItemSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject plantPrefab; // Prefab with SpriteRenderer
    [SerializeField] private float lerpSpeed = 10f; // Speed of lerping
    private GameObject currentPlant; // Reference to instantiated plant
    private Camera mainCamera;
    private SpriteRenderer plantRenderer; // To get the plant's size
    private Vector3 targetPosition; // The position we lerp to
    private bool isDragging = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isDragging && currentPlant != null)
        {
            // Smoothly move toward the target position
            currentPlant.transform.position = Vector3.Lerp(
                currentPlant.transform.position,
                targetPosition,
                Time.deltaTime * lerpSpeed
            );
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Convert mouse position to world position at the start
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Keep it on the correct plane

        // Instantiate the plant at the mouse position
        currentPlant = Instantiate(plantPrefab, mouseWorldPos, Quaternion.identity);
        plantRenderer = currentPlant.GetComponent<SpriteRenderer>();
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentPlant != null)
        {
            // Convert mouse position to world position
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; // Keep on correct plane

            // Clamp position within screen bounds
            targetPosition = ClampToScreenBounds(mouseWorldPos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private Vector3 ClampToScreenBounds(Vector3 position)
    {
        if (plantRenderer == null) return position; // Safety check

        // Get the plant's size in world units
        float plantWidth = plantRenderer.bounds.extents.x; // Half-width
        float plantHeight = plantRenderer.bounds.extents.y; // Half-height

        // Get screen bounds in world coordinates
        Vector3 minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Adjusted clamping: Ensure edges stay within bounds
        position.x = Mathf.Clamp(position.x, minScreenBounds.x + plantWidth, maxScreenBounds.x - plantWidth);
        position.y = Mathf.Clamp(position.y, minScreenBounds.y + plantHeight, maxScreenBounds.y - plantHeight);

        return position;
    }
}
