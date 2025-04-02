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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Spawn the plant in world space
        currentPlant = Instantiate(plantPrefab, mouseWorldPos, Quaternion.identity);

        GameObject clickedUIObject = eventData.pointerCurrentRaycast.gameObject;
        SetPlantImage(clickedUIObject);

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

    private void SetPlantImage(GameObject clickedItemUnit) {
        Transform itemImgTransform = clickedItemUnit.transform.Find("ItemImg");
        Image plantImageUI =
            itemImgTransform ? itemImgTransform.GetComponent<Image>() : null;

        // Set the sprite of the newly instantiated plant to the UI sprite
        if (currentPlant != null && plantImageUI != null)
        {
            Sprite plantSprite = plantImageUI.sprite;
            SpriteRenderer plantRenderer = currentPlant.GetComponent<SpriteRenderer>();
            plantRenderer.sprite = plantSprite;
        } else {
            print("could not set plant image");
        }
    }
}