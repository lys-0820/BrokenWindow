using UnityEngine;

[CreateAssetMenu(fileName = "DecorationData", menuName = "Scriptable Objects/DecorationData")]
public class DecorationData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite image;
}
