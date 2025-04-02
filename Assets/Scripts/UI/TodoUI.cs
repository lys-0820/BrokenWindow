using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class TodoUI : MonoBehaviour
{
    public static TodoUI Instance;

    public GameObject todoItemPrefab;
    public Transform contentPanel;
    #region temp
    public Button BtAdd;
    public Button BtRemove;
    public Button BtTime;
    public Button BtPage1;
    public Button BtPage2;
    public Button BtPage3;
    #endregion
    private Dictionary<string, GameObject> todoTextMap = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        //InitUI(TodoManager.Instance.GetCurrentPage());
        #region temp
        BtAdd.onClick.AddListener(TodoManager.Instance.NotifyPlantPlaced);
        BtRemove.onClick.AddListener(TodoManager.Instance.NotifyPlantRemove);
        BtTime.onClick.AddListener(TodoManager.Instance.NotifyTimeJumpUsed);
        BtPage1.onClick.AddListener(() => TodoManager.Instance.SwitchPage(0));
        BtPage2.onClick.AddListener(() => TodoManager.Instance.SwitchPage(1));
        BtPage3.onClick.AddListener(() => TodoManager.Instance.SwitchPage(2));
        BtPage2.interactable = false;
        BtPage3.interactable = false;
        #endregion
    }
    public void InitUI(TodoPageData tasks)
    {
        Debug.Log(tasks);
        //clear all the child
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        todoTextMap.Clear();
        foreach (var task in tasks.todoList)
        {
            GameObject item = Instantiate(todoItemPrefab, contentPanel);
            ShowTaskText(item, task);
            //DesText.name = task.id;
            todoTextMap[task.id] = item;
            
        }
    }

    public void MarkTodoComplete(string id)
    {
        if (todoTextMap.TryGetValue(id, out GameObject TaskItem))
        {
            Debug.Log(TaskItem);

            StartCoroutine(ShowComplete(TaskItem, TodoManager.Instance.GetTaskIsDone(id)));
        }
    }
    private void ShowTaskText(GameObject item, TodoData task)
    {
        TMP_Text DesText = item.transform.Find("todoText").GetComponent<TMP_Text>();
        TMP_Text InnerText = item.transform.Find("innerText").GetComponent<TMP_Text>();
        DesText.text = task.description;
        InnerText.text = task.innerMonologue;
        if (task.isCompleted)
        {
            DesText.fontStyle |= FontStyles.Strikethrough;
            DesText.color = Color.gray;
            InnerText.gameObject.SetActive(true);
        }
        else
        {
            DesText.fontStyle &= ~FontStyles.Strikethrough;
            DesText.color = Color.black;
            InnerText.gameObject.SetActive(false);
        }
    }
    public IEnumerator ShowComplete(GameObject taskItem, bool isTaskCompleted)
    {
        // 获取两个子Text组件（确保命名一致或用tag也行）
        TMP_Text taskText = taskItem.transform.Find("todoText").GetComponent<TMP_Text>();
        TMP_Text hiddenMeaningText = taskItem.transform.Find("innerText").GetComponent<TMP_Text>();

        // 修改主任务文字样式
        taskText.fontStyle |= FontStyles.Strikethrough;
        taskText.color = Color.gray;

        yield return new WaitForSeconds(0.5f);

        // 设置并显示 hidden meaning
        hiddenMeaningText.gameObject.SetActive(true);
        //hiddenMeaningText.text = hiddenMeaning;
        hiddenMeaningText.alpha = 0f;
        hiddenMeaningText.gameObject.SetActive(true);

        // 淡入动画
        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            hiddenMeaningText.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        hiddenMeaningText.alpha = 1f;

    }
    public void UnlockNewPage()
    {
        if(BtPage2.interactable == false)
        {
            BtPage2.interactable = true;
        }
        else if (BtPage3.interactable == false)
        {
            BtPage3.interactable = true;
        }
    }
}
