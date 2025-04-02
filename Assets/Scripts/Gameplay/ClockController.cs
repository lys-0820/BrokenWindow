using UnityEngine;
using UnityEngine.EventSystems;

public class ClockTicklingRotation : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Clock Hands")]
    public Transform hourHand;  
    public Transform minuteHand;  

    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private Vector3 clockCenter;
    private CircleCollider2D clockCollider;
    
    [Header("Clock Settings")]
    public float rotationSpeed = 10f;

    [Header("Animation")]
    public Animator clockAnimator; // Reference to Animator component

    void Start()
    {
        clockCollider = GetComponent<CircleCollider2D>();
        clockCenter = clockCollider.bounds.center;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsMouseOverCollider(eventData))
            return;

        isDragging = true;
        lastMousePosition = GetMouseWorldPosition();

        // Start animation
        if (clockAnimator != null) {
            clockAnimator.SetBool("tickle", true);
            print("tickling true");
        } else {
            print("NO ANIMATOR!!!!!!!!!!!!");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || !IsMouseOverCollider(eventData))
            return;

        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 delta = mousePosition - lastMousePosition;

        if (delta.magnitude > 0.001f)
        {
            float scaledRotation = delta.magnitude * rotationSpeed;
            if (minuteHand != null)
                minuteHand.RotateAround(clockCenter, Vector3.forward, -scaledRotation);

            if (hourHand != null)
                hourHand.RotateAround(clockCenter, Vector3.forward, -scaledRotation / 12f);

            lastMousePosition = mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // Stop animation
        if (clockAnimator != null) {
            print("tickling false");
            clockAnimator.SetBool("tickle", false);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenMousePosition = Input.mousePosition;
        screenMousePosition.z = 0;
        return Camera.main.ScreenToWorldPoint(screenMousePosition);
    }

    private bool IsMouseOverCollider(PointerEventData eventData)
    {
        Vector3 worldMousePosition = GetMouseWorldPosition();
        return clockCollider.OverlapPoint(worldMousePosition);
    }
}