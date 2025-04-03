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
    private Transform contentPanel;

    public Button BtPage1;
    public Button BtPage2;
    public Button BtPage3;

    private Dictionary<string, GameObject> todoTextMap = new Dictionary<string, GameObject>();

    public AudioSource pencilAudioSource;

    public Button BtClose;
    public Sprite bgPage1;
    public Sprite bgPage2;
    public Sprite bgPage3;
    public Image bgImg;
    public GameObject panelPage1;
    public GameObject panelPage2;
    public GameObject panelPage3;

    public GameObject stampObj;

    public AudioSource flipAudioSource;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        BtPage2.interactable = false;
        BtPage3.interactable = false;
        contentPanel = panelPage1.transform;
    }

    private void Start()
    {
        //InitUI(TodoManager.Instance.GetCurrentPage());
        BtPage1.onClick.AddListener(() => StartCoroutine(TodoManager.Instance.SwitchPage(0)));
        BtPage2.onClick.AddListener(() => StartCoroutine(TodoManager.Instance.SwitchPage(1)));
        BtPage3.onClick.AddListener(() => StartCoroutine(TodoManager.Instance.SwitchPage(2)));
        BtClose.onClick.AddListener(HideTodoPanel);
        transform.gameObject.SetActive(false);
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
        if (tasks.phase == 1)
        {
            BtPage2.interactable = true;
        }
        else if (tasks.phase == 2)
        {
            BtPage2.interactable = true;
            BtPage3.interactable = true;
        }
        switch (tasks.phase)
        {
            case 0:
                bgImg.sprite = bgPage1;
                panelPage1.SetActive(true);
                panelPage2.SetActive(false);
                panelPage3.SetActive(false);
                contentPanel = panelPage1.transform;
                break;
            case 1:
                bgImg.sprite = bgPage2;
                panelPage1.SetActive(false);
                panelPage2.SetActive(true);
                panelPage3.SetActive(false);
                contentPanel = panelPage2.transform;
                break;
            case 2:
                bgImg.sprite = bgPage3;
                panelPage1.SetActive(false);
                panelPage2.SetActive(false);
                panelPage3.SetActive(true);
                contentPanel = panelPage3.transform;
                break;

        }
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
        TMP_Text taskText = taskItem.transform.Find("todoText").GetComponent<TMP_Text>();
        TMP_Text hiddenMeaningText = taskItem.transform.Find("innerText").GetComponent<TMP_Text>();
        yield return new WaitForSeconds(0.5f);

        pencilAudioSource.Play();
        yield return new WaitForSeconds(0.2f);
        // change todo text
        taskText.fontStyle |= FontStyles.Strikethrough;
        taskText.color = Color.gray;

        yield return new WaitForSeconds(0.5f);

        hiddenMeaningText.gameObject.SetActive(true);

        // typewriter effect
        string fullText = hiddenMeaningText.text;
        hiddenMeaningText.text = "";
        hiddenMeaningText.gameObject.SetActive(true);

        float typeSpeed = 0.05f; // interval between each character
        for (int i = 0; i <= fullText.Length; i++)
        {
            hiddenMeaningText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

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

    public void ShowTodoPanel()
    {
        transform.gameObject.SetActive(true);
    }
    public void HideTodoPanel()
    {
        transform.gameObject.SetActive(false);
    }

    public IEnumerator MakeStamp()
    {
        stampObj.GetComponent<Stamp>().Play();
        yield return new WaitForSeconds(6f);
    }
    public void PlayFlipSound()
    {
        flipAudioSource.Play();
    }
}
