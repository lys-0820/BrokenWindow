using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum PlantMaturity
    {
        Baby,
        Child,
        Adult
    }

    protected Camera mainCamera;
    protected Vector3 targetPosition;
    protected bool isDragging = false;
    private Vector3 originalPosition;
    private PlantDropZone originalDropZone;
    private PlantDropZone[] allDropZones;
    private PlantMaturity maturity = PlantMaturity.Baby;
    private Vector3 originalScale;
    private PlantSoundManager soundManager;

    public bool justSpawned = false;
    private int dayCycles = 0;
    public PlantType plantType = PlantType.Potted;
    public PlantGrowthData growthData;
    public GameObject particleEffectPrefab;

    [SerializeField] protected float lerpSpeed = 10f; // Lerp speed for smooth dragging
    public Vector3 dropZoneOffset = new Vector3(0, 0.2f, 0);

    protected SpriteRenderer spriteRenderer;
    private Animator animator;

    public void Init(Camera camera)
    {
        mainCamera = camera;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        allDropZones = FindObjectsByType<PlantDropZone>(FindObjectsSortMode.None);
        soundManager = FindFirstObjectByType<PlantSoundManager>();
    }

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        ClockController.OnDayPassed += HandleDayPassed;

        // plant sprite is set in draggableItemSpawner for items without growthData
        if (growthData == null)
        {
            return;
        }

        if (growthData.babySprite == null)
        {
            spriteRenderer.sprite = growthData.adultSprite;
            maturity = PlantMaturity.Adult;
        }
        else
        {
            spriteRenderer.sprite = growthData.babySprite;
        }
    }

    void OnDestroy()
    {
        ClockController.OnDayPassed -= HandleDayPassed;
    }

    protected virtual void Update()
    {
        if (isDragging)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * lerpSpeed);
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        isDragging = true;
        SetDraggingVisuals(true);
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
        SetDraggingVisuals(false);
        HideAllDropZones();

        // Check for the nearest drop zone
        Collider2D dropZoneCollider = GetNearestDropZone();
        PlantDropZone plantDropZone = dropZoneCollider
            ? dropZoneCollider.GetComponent<PlantDropZone>()
            : null;
        if (plantDropZone == null)
        {
            print("illegal plant drop");
            HandleIllegalPlantDrop();
            return;
        }

        if (plantDropZone.isDiscardZone)
        {
            print("discard zoneeee");
            TodoManager.Instance.NotifyPlantRemove();
            soundManager.PlayDiscardedClip();
            Destroy(gameObject);
            return;
        }

        if (plantDropZone.placedPlant == null && plantDropZone.plantType == plantType)
        {
            plantDropZone.placedPlant = gameObject;
            transform.position = dropZoneCollider.transform.position + dropZoneOffset; // Snap to drop zone
            justSpawned = false;
            TodoManager.Instance.NotifyPlantPlaced();
            soundManager.PlayPlacedClip();
            TriggerParticleEffect(transform.position);
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

    private void ShowValidDropZones()
    {
        foreach (var dropZone in allDropZones)
        {
            if (dropZone.plantType == plantType || dropZone.isDiscardZone)
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

    private void HandleDayPassed()
    {
        print("DAY PASSED!!!!!!!!!!!!!!!!!");
        dayCycles++;

        if (growthData == null)
        {
            return;
        }

        // adults don't have growing to do
        if (maturity == PlantMaturity.Adult)
        {
            return;
        }

        // child to adult
        else if (maturity == PlantMaturity.Child)
        {
            if (dayCycles >= growthData.childDuration)
            {
                maturity = PlantMaturity.Adult;
                spriteRenderer.sprite = growthData.adultSprite;
                if (growthData.adultAnimation != null)
                {
                    animator.runtimeAnimatorController = growthData.adultAnimation;
                }
            }
            // baby to child
        }
        else if (maturity == PlantMaturity.Baby)
        {
            if (dayCycles >= growthData.babyDuration)
            {
                maturity = PlantMaturity.Child;
                spriteRenderer.sprite = growthData.childSprite;
            }
        }
    }

    private void SetDraggingVisuals(bool isDragging)
    {
        if (isDragging)
        {
            originalScale = transform.localScale;
            transform.localScale *= 0.9f;
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0.6f;
                spriteRenderer.color = color;
                // dragVisualOffset = new Vector3(0f, -spriteRenderer.bounds.extents.y / 2, 0f);
            }
        }
        else
        {
            transform.localScale = originalScale;
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
                // dragVisualOffset = Vector3.zero;
            }
        }
    }

    private void TriggerParticleEffect(Vector3 position)
    {
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, position, Quaternion.identity);
        }
    }
}
