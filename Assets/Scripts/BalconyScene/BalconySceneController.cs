using UnityEngine;
using UnityEngine.UI;
public class BalconySceneController : MonoBehaviour
{
    [SerializeField] private Button BtHome;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BtHome.onClick.AddListener(() =>
        {
            SceneController.Instance.loadScene("BuildingScene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
