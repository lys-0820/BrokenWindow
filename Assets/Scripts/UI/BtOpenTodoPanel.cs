using UnityEngine;
using UnityEngine.UI;
public class BtOpenTodoPanel : MonoBehaviour
{
    [SerializeField] private Button BtOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BtOpen.onClick.AddListener(() =>
        {
            TodoUI.Instance.ShowTodoPanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
