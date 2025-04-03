using UnityEngine;

public enum PlantType
{
    Potted,
    Hanging
}

[CreateAssetMenu(menuName = "Scriptable Objects/Plant Data")]
public class PlantData : PlaceableItemData
{
    public PlantType plantType;
    public PlantGrowthData growthData;
    public float scaleFactor = 1f;
}
