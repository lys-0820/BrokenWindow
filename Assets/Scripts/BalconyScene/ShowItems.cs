using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItems : MonoBehaviour
{
    public List<PlantData> plantDataList; 
    public List<PlaceableItemData> holderDataList; 
    public List<PlaceableItemData> decorationDataList; 
    
    //public GameObject plantPrefab; 
    public GameObject itemPrefab; 
    public GameObject decorationPrefab; 
    public Transform parentTransform; // parent object of the prefabs

    void Start()
    {
        InitializeItems(plantDataList); 
        InitializeItems(holderDataList);
        InitializeItems(decorationDataList);
    }

    void Update()
    {
        
    }

    public void OnTagSelected(string tag) 
    {
        ClearParent(); 

        switch (tag)
        {
            case "plant":
                InitializeItems(plantDataList);
                break;
            case "holder":
                InitializeItems(holderDataList);
                break;
            case "decoration":
                InitializeItems(decorationDataList);
                break;
        }
    }

    private void ClearParent() 
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitializeItems<T>(List<T> dataList) 
    {
        foreach (var data in dataList)
        {
            if (((PlaceableItemData)(object)data).isUnlocked)
            {
                GameObject itemInstance = null;
                itemInstance = Instantiate(itemPrefab, parentTransform);
                // assign value to UI
                Text itemNameText = itemInstance.transform.Find("ItemName").GetComponent<Text>();
                Text itemDescriptionText = itemInstance.transform.Find("ItemDes").GetComponent<Text>();
                itemNameText.text = ((PlaceableItemData)(object)data).name;
                itemDescriptionText.text = ((PlaceableItemData)(object)data).description;
                Image itemImage = itemInstance.transform.Find("ItemImg").GetComponent<Image>();
                itemImage.sprite = ((PlaceableItemData)(object)data).image;
            }

        }
    }
}
