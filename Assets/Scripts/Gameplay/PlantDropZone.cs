using UnityEngine;

public class PlantDropZone : MonoBehaviour
{
    public GameObject placedPlant;

    // Allows removing a plant if needed
    public void RemovePlant()
    {
        placedPlant = null;
    }
}