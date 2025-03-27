using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRect;
    private bool isDragging = false;
    private Vector2 targetPosition; // The position we lerp to
    
    // Drag Lerp speed
    [SerializeField] private float lerpSpeed = 10f;

    // Grid snap settings
    [SerializeField] private float gridSize = 100f; // Size of each grid cell (e.g., 100x100 units)

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // Get the parent Canvas
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Lerp the UI element toward the target position
        if (isDragging)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * lerpSpeed);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert mouse position to local UI space
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePosition);

        if (!IsMouseInsideCanvas(mousePosition)) return; // Stop dragging if out of bounds

        // Get canvas bounds
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float elementWidth = rectTransform.rect.width * rectTransform.lossyScale.x;
        float elementHeight = rectTransform.rect.height * rectTransform.lossyScale.y;

        // Calculate min and max bounds
        float minX = -canvasWidth / 2 + elementWidth / 2;
        float maxX = canvasWidth / 2 - elementWidth / 2;
        float minY = -canvasHeight / 2 + elementHeight / 2;
        float maxY = canvasHeight / 2 - elementHeight / 2;

        // Snap mouse position to grid (round to nearest grid point)
        float snappedX = Mathf.Round(mousePosition.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(mousePosition.y / gridSize) * gridSize;

        // Now clamp the snapped position within canvas bounds
        targetPosition.x = Mathf.Clamp(snappedX, minX, maxX);
        targetPosition.y = Mathf.Clamp(snappedY, minY, maxY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private bool IsMouseInsideCanvas(Vector2 mousePosition)
    {
        return canvasRect.rect.Contains(mousePosition);
    }
}