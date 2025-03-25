using UnityEngine;
using UnityEngine.UI;
public class BuildingSceneController : MonoBehaviour
{
    [SerializeField] private Button BtScene1;
    [SerializeField] private Button BtScene2;
    [SerializeField] private Button BtScene3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BtScene1.onClick.AddListener(() =>
        {
            SceneController.Instance.loadScene("scene1");
        });
        BtScene2.onClick.AddListener(() =>
        {
            SceneController.Instance.loadScene("scene2");
        });
        BtScene3.onClick.AddListener(() =>
        {
            SceneController.Instance.loadScene("scene3");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
