using UnityEngine;

public enum PlantType
{
    Potted,
    Hanging,
    Wall
}

[CreateAssetMenu(menuName = "Scriptable Objects/Plant Data")]
public class PlantData : PlaceableItemData
{
    public PlantType plantType;
}
