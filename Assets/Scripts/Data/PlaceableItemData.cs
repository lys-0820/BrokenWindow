using UnityEngine;

public enum ItemType
{
    Plant,
    Holder,
    Decoration
}

[CreateAssetMenu(fileName = "PlaceableItemData", menuName = "Scriptable Objects/PlaceableItemData")]
public class PlaceableItemData : ScriptableObject
{
    public string plantName;
    public string description;
    public Sprite image;
    public ItemType itemType;
    public bool isUnlocked;
}
