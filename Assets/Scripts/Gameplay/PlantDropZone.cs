using UnityEngine;

public class PlantDropZone : MonoBehaviour
{
    public GameObject placedPlant;
    public PlantType plantType;

    public void RemovePlant()
    {
        placedPlant = null;
    }
}