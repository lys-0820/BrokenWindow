using UnityEngine;

[CreateAssetMenu(fileName = "HolderData", menuName = "Scriptable Objects/HolderData")]
public class HolderData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite image;
}
