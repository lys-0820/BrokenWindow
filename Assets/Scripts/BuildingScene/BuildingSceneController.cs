using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BuildingSceneController : MonoBehaviour
{
    [SerializeField] private Button BtScene1;
    [SerializeField] private Button BtScene2;
    [SerializeField] private Button BtScene3;
    [SerializeField] private Button BtLevelUp;
    public GameObject levelupObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelupObj.SetActive(false);
        BtScene1.onClick.AddListener(() =>
        {
            SceneController.Instance.loadScene("testSceneForTodo");
        });
        //BtScene2.onClick.AddListener(() =>
        //{
        //    SceneController.Instance.loadScene("scene2");
        //});
        //BtScene3.onClick.AddListener(() =>
        //{
        //    SceneController.Instance.loadScene("scene3");
        //});
        BtLevelUp.onClick.AddListener(() =>
        {
            StartCoroutine(ShowLevelUp());
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ShowLevelUp()
    {
        levelupObj.SetActive(true);
        yield return new WaitForSeconds(3f);
        levelupObj.SetActive(false);
    }
}
