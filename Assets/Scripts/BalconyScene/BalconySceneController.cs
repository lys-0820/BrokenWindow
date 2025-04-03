using UnityEngine;
using UnityEngine.UI;
public class BalconySceneController : MonoBehaviour
{
    [SerializeField] private Button BtHome;
    [SerializeField] private Button BtBalcony;
    public CameraMover cameraMover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BtHome.onClick.AddListener(() =>
        {
            cameraMover.MoveToCityView();
            // SceneController.Instance.loadScene("BuildingScene");
        });

        BtBalcony.onClick.AddListener(() =>
        {
            cameraMover.MoveToBalconyView();
        });

        TodoUI.Instance.InitUI(TodoManager.Instance.currentPage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
