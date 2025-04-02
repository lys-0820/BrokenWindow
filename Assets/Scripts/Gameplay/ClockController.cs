using UnityEngine;
using UnityEngine.EventSystems;

public class ClockTicklingRotation : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Clock Hands")]
    public Transform hourHand;  // Reference to the hour hand
    public Transform minuteHand;  // Reference to the minute hand

    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private Vector3 clockCenter;
    private CircleCollider2D clockCollider;

    [Header("Clock Settings")]
    public float rotationSpeed = 10f; // The speed at which the clock hands rotate per tick (adjustable in the inspector)
    private float accumulatedRotation = 0f;  // The accumulated rotation amount

    // Start is called before the first frame update
    void Start()
    {
        // Get the center of the clock (use the CircleCollider2D center)
        clockCenter = GetComponent<CircleCollider2D>().bounds.center;
        clockCollider = GetComponent<CircleCollider2D>();
    }

    // Called when dragging starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsMouseOverCollider(eventData))
            return;

        isDragging = true;
        lastMousePosition = Input.mousePosition;
        accumulatedRotation = 0f;  // Reset accumulated rotation
    }

    // Called while dragging
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || !IsMouseOverCollider(eventData))
            return;

        // Calculate the movement from the center of the clock to the mouse position (both x and y axis)
        Vector3 mousePosition = Input.mousePosition;
        Vector3 delta = mousePosition - lastMousePosition;

        // Only update if movement exceeds the threshold
        if (delta.magnitude > 0.1f)
        {
            // The amount of rotation will be based on the distance dragged, scaled by the speed factor
            accumulatedRotation += delta.magnitude * rotationSpeed * Time.deltaTime;

            // Update the clock hands based on the accumulated rotation
            UpdateClockHands();

            // Update the last mouse position for the next frame
            lastMousePosition = mousePosition;
        }
    }

    // Called when dragging stops
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    // Updates the clock hands based on accumulated drag
    private void UpdateClockHands()
    {
        // Rotate the minute hand based on accumulated rotation
        if (minuteHand != null)
        {
            // Rotate clockwise (inverting the sign of rotation for clockwise movement)
            minuteHand.RotateAround(clockCenter, Vector3.forward, -accumulatedRotation);
        }

        // Rotate the hour hand (scaled by 12, as 1 full rotation of minute hand = 1 hour)
        if (hourHand != null)
        {
            // Rotate clockwise (inverting the sign of rotation for clockwise movement)
            hourHand.RotateAround(clockCenter, Vector3.forward, -accumulatedRotation / 12f);
        }

        // Reset the accumulated rotation after applying the rotation
        accumulatedRotation = 0f;
    }

    // Helper function to check if the mouse is over the clock collider
    private bool IsMouseOverCollider(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;

        // Convert the mouse position to world space
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        
        // Check if the world position is inside the clock's collider
        return clockCollider.OverlapPoint(worldMousePosition);
    }
}