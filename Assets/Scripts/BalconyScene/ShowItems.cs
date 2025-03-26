using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItems : MonoBehaviour
{
    public List<PlantData> plantDataList; 
    public List<HolderData> holderDataList; // 新增的holder数据列表
    public List<DecorationData> decorationDataList; // 新增的decoration数据列表
    
    public GameObject plantPrefab; // 预制体引用
    public GameObject holderPrefab; // 新增的holder预制体引用
    public GameObject decorationPrefab; // 新增的decoration预制体引用
    public Transform parentTransform; // 预制体的父物体

    void Start()
    {
        InitializeItems(plantDataList); // 默认初始化植物
    }

    void Update()
    {
        
    }

    public void OnTagSelected(string tag) // 新增的方法处理标签选择
    {
        ClearParent(); // 清空父物体下的所有对象

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

    private void ClearParent() // 新增的方法清空父物体
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitializeItems<T>(List<T> dataList) // 新增的泛型方法初始化对象
    {
        foreach (var data in dataList)
        {
            GameObject itemInstance = null;
            if (data is PlantData)
            {
                itemInstance = Instantiate(plantPrefab, parentTransform);
                // 获取 UI 组件并赋值
                Text plantNameText = itemInstance.transform.Find("PlantName").GetComponent<Text>(); 
                Text plantDescriptionText = itemInstance.transform.Find("PlantDes").GetComponent<Text>(); 
                plantNameText.text = ((PlantData)(object)data).name; // 设置植物名称
                plantDescriptionText.text = ((PlantData)(object)data).description;
                Image plantImage = itemInstance.transform.Find("PlantImg").GetComponent<Image>();
                plantImage.sprite = ((PlantData)(object)data).image;
            }
            else if (data is HolderData)
            {
                itemInstance = Instantiate(holderPrefab, parentTransform);
                // 获取 UI 组件并赋值
                Text holderNameText = itemInstance.transform.Find("HolderName").GetComponent<Text>(); 
                Text holderDescriptionText = itemInstance.transform.Find("HolderDes").GetComponent<Text>(); 
                holderNameText.text = ((HolderData)(object)data).name; // 设置植物名称
                holderDescriptionText.text = ((HolderData)(object)data).description;
                Image holderImage = itemInstance.transform.Find("HolderImg").GetComponent<Image>();
                holderImage.sprite = ((HolderData)(object)data).image;
            }
            else if (data is DecorationData)
            {
                itemInstance = Instantiate(decorationPrefab, parentTransform);
                // 获取 UI 组件并赋值
                Text decorationNameText = itemInstance.transform.Find("DecorationName").GetComponent<Text>(); 
                Text decorationDescriptionText = itemInstance.transform.Find("DecorationDes").GetComponent<Text>(); 
                decorationNameText.text = ((DecorationData)(object)data).name; // 设置植物名称
                decorationDescriptionText.text = ((DecorationData)(object)data).description;
                Image decorationImage = itemInstance.transform.Find("DecorationImg").GetComponent<Image>();
                decorationImage.sprite = ((DecorationData)(object)data).image;
            }
        }
    }
}
