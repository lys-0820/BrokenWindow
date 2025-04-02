using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject plantPrefab;
    private GameObject currentPlant;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // Ensure this object does not block drag input in Scroll View
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Spawn the plant in world space
        currentPlant = Instantiate(plantPrefab, mouseWorldPos, Quaternion.identity);

        // Start dragging the newly created item
        DraggableItem draggable = currentPlant.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.justSpawned = true;
            draggable.Init(mainCamera);
            draggable.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentPlant == null)
        {
            Debug.LogError("OnDrag called but no currentPlant exists. Check if OnBeginDrag is working.");
            return;
        }

        DraggableItem draggable = currentPlant.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.OnDrag(eventData);
        }
        else
        {
            Debug.LogError("DraggableItem component missing on the spawned plant prefab.");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentPlant != null)
        {
            DraggableItem draggable = currentPlant.GetComponent<DraggableItem>();
            if (draggable != null)
            {
                draggable.OnEndDrag(eventData);
            }
        }
    }
}