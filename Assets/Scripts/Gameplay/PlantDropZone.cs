using UnityEngine;

public class PlantDropZone : MonoBehaviour
{
    public GameObject placedPlant;
    public PlantType plantType = PlantType.Potted;
    public bool isDiscardZone = false;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void ShowDropZone()
    {
        spriteRenderer.enabled = true;
    }

    public void HideDropZone()
    {
        spriteRenderer.enabled = false;
    }
}