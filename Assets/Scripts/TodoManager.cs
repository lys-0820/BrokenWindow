using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TodoManager : MonoBehaviour
{
    public static TodoManager Instance;

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

    public int plantCount;
    public int timeJumpCount;

    // todo manager
    public List<TodoPageData> todoPageList;
    public TodoPageData currentPage;
    private int currentPhase;
    private int finishedTaskCount;


    void Start()
    {
        plantCount = 0;
        timeJumpCount = 0;
        currentPhase = 0;
        finishedTaskCount = 0;
        foreach (var page in todoPageList)
        {
            foreach (var task in page.todoList)
            {
                task.isCompleted = false;
            }
        }
        currentPage = todoPageList[0];
        Debug.Log(currentPage);
        //TodoUI.Instance.InitUI(currentPage);


    }

    public void NotifyPlantPlaced()
    {
        plantCount++;
        CheckTasks(TaskType.Plant, plantCount);
    }
    public void NotifyPlantRemove()
    {
        if (plantCount > 0)
        {
            plantCount--;
            CheckTasks(TaskType.Plant, plantCount);
        }
    }
    public void NotifyTimeJumpUsed()
    {
        timeJumpCount++;
        CheckTasks(TaskType.TimeJump, timeJumpCount);
    }
    public void SwitchPage(int index)
    {
        if (index >= 0 && index < todoPageList.Count)
        {
            currentPage = todoPageList[index];
            TodoUI.Instance.InitUI(currentPage);
        }
    }
    private void CheckTasks(TaskType type, int currentCount)
    {
        foreach (var task in currentPage.todoList)
        {
            if (task.type == type && !task.isCompleted && currentCount >= task.targetCount)
            {
                
                Debug.Log($"TODO Complete: {task.description}");
                TodoUI.Instance.ShowTodoPanel();
                TodoUI.Instance.MarkTodoComplete(task.id);
                task.isCompleted = true;
                finishedTaskCount++;
            }
        }
        if(finishedTaskCount>=currentPage.todoList.Count&& currentPhase<todoPageList.Count-1)
        {
            TodoUI.Instance.UnlockNewPage();
            currentPage = todoPageList[++currentPhase];
            finishedTaskCount = 0;
        }
    }
    public TodoPageData GetCurrentPage()
    {
        return currentPage;
    }

    public bool GetTaskIsDone(string id)
    {
        foreach (var page in todoPageList)
        {
            foreach (var task in page.todoList)
            {
                if(task.id == id)
                {
                    return task.isCompleted;
                }
            }
        }
        return false;
    }

}
