using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 可以在这里初始化一些东西
    }

    // Update is called once per frame
    void Update()
    {
        // 可以在这里处理每帧更新的逻辑
    }

    // 添加一个公共方法来切换场景
    public void LoadScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
    }
}
