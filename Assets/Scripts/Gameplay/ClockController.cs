using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClockController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Clock Hands")]
    public Transform hourHand;  
    public Transform minuteHand;  

    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private Vector3 clockCenter;
    private CircleCollider2D clockCollider;

    private float totalHourRotation = 0f; // Accumulates for day passing
    private bool IsDaytime = true;
    
    [Header("Clock Settings")]
    public float rotationSpeed = 10f;

    [Header("Animation")]
    public Animator clockAnimator; // Reference to Animator component

    public delegate void HalfDayPassed();
    public static event HalfDayPassed OnHalfDayPassed;

    public delegate void DayPassed();
    public static event DayPassed OnDayPassed;

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
            float scaledRotation = -delta.magnitude * rotationSpeed;
            if (minuteHand != null)
                minuteHand.RotateAround(clockCenter, Vector3.forward, scaledRotation);

            if (hourHand != null) {
                float hourRotation = scaledRotation / 12f;
                hourHand.RotateAround(clockCenter, Vector3.forward, hourRotation);
                totalHourRotation += Mathf.Abs(hourRotation);

                // Check if the hour hand has completed a full circle (360 degrees)
                if (totalHourRotation >= 180f)
                {
                    totalHourRotation = 0f; // Reset counter
                    OnHalfDayPassed?.Invoke(); // Notify observers
                    IsDaytime = !IsDaytime; // Alternate between day and night

                    // If it's day again notify a full day has passed.
                    if (IsDaytime) {
                        OnDayPassed?.Invoke();
                    }
                }
            }

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