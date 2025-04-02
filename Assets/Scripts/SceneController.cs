using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 单例实例
    public static SceneController Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // load scenes
    public void loadScene(string sceneName)
    {
        int currentPhase = TodoManager.Instance.GetCurrentPhase();
        Debug.Log("currentPhase: " + currentPhase);
        SceneManager.LoadScene(sceneName);
    }

    // initialize the instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
