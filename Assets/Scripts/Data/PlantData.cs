using UnityEngine;

public enum PlantType
{
    Potted,
    Hanging,
    Wall
}

[CreateAssetMenu(menuName = "Scriptable Objects/Plant Data")]
public class PlantData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite image;
    public PlantType plantType;
}
