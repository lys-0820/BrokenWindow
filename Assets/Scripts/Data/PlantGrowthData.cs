using UnityEngine;

[CreateAssetMenu(fileName = "PlantGrowthData", menuName = "Scriptable Objects/PlantGrowthData")]
public class PlantGrowthData : ScriptableObject
{
    public Sprite babySprite;
    public Sprite childSprite;
    public Sprite adultSprite;
    public RuntimeAnimatorController adultAnimation; // Optional animation for adult stage
    public int babyDuration = 1;  // Time before changing to child stage
    public int childDuration = 1; // Time before changing to adult stage
}