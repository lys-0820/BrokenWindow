using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TodoUI : MonoBehaviour
{
    public static TodoUI Instance;

    public GameObject todoItemPrefab;
    public Transform contentPanel;

    private Dictionary<string, Text> todoTextMap = new Dictionary<string, Text>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        InitUI(TodoManager.Instance.todoList);
    }
    public void InitUI(List<TodoData> tasks)
    {
        foreach (var task in tasks)
        {
            GameObject item = Instantiate(todoItemPrefab, contentPanel);
            Text text = item.GetComponentInChildren<Text>();
            text.text = task.description;
            text.name = task.id;
            todoTextMap[task.id] = text;
        }
    }

    public void MarkTodoComplete(string id)
    {
        if (todoTextMap.TryGetValue(id, out Text text))
        {
            StartCoroutine(ShowComplete(text));
        }
    }
    private IEnumerator ShowComplete(Text text)
    {
        text.color = Color.gray;
        text.text += " ✓";
        yield return new WaitForSeconds(1.5f);
        // move to the last
        Transform itemTransform = text.transform;
        itemTransform.SetAsLastSibling();
    }
}
