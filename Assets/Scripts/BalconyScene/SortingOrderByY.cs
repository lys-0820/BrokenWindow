using UnityEngine;

public class SortingOrderByY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
    }

    void Update()
    {
        UpdateSortingOrder();
    }

    void UpdateSortingOrder()
    {
        // Invert Y position so lower objects appear in front
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}