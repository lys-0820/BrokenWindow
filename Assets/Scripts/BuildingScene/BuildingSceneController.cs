using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BuildingSceneController : MonoBehaviour
{
    [SerializeField] private Button BtScene1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BtScene1.onClick.AddListener(() =>
        {
            SceneController.Instance.loadScene("BalconyScene");
        });

    }

}
