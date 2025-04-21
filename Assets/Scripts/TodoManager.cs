using System.Collections;
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
            currentPage = todoPageList[0];
            Debug.Log(currentPage);
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

        //TodoUI.Instance.InitUI(currentPage);

        ClockController.OnDayPassed += HandleDayPassed;
    }

    void OnDestroy()
    {
        ClockController.OnDayPassed -= HandleDayPassed;
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

    public void HandleDayPassed()
    {
        timeJumpCount++;
        CheckTasks(TaskType.TimeJump, timeJumpCount);
    }

    public IEnumerator SwitchPage(int index)
    {
        Debug.Log("switch to page " + index);
        if (index >= 0 && index < todoPageList.Count)
        {
            TodoUI.Instance.PlayFlipSound();
            currentPage = todoPageList[index];
            TodoUI.Instance.InitUI(currentPage);
            Debug.Log("switch to page " + index);
        }
        yield return null;
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
        if(finishedTaskCount>=currentPage.todoList.Count)
        {
            StartCoroutine(FinishOnePage());
            TodoUI.Instance.PlayBtHomeAnim();
        }
    }
    private IEnumerator FinishOnePage()
    {
        Debug.Log("current phase is:" + currentPhase);
        if (currentPhase < todoPageList.Count - 1)
        {
            //not finished all the task
            yield return StartCoroutine(TodoUI.Instance.MakeStamp());
            TodoUI.Instance.UnlockNewPage();
            currentPage = todoPageList[++currentPhase];
            finishedTaskCount = 0;
            yield return StartCoroutine(SwitchPage(currentPhase));
            //SwitchPage(currentPhase);
            CheckTasks(TaskType.Plant, plantCount);
            CheckTasks(TaskType.TimeJump, timeJumpCount);
            
        }
        else
        {
            //finished all the task
            StartCoroutine(TodoUI.Instance.MakeStamp());
            currentPhase++;
            finishedTaskCount = 0;
        }
        Debug.Log("current phase is:" + currentPhase);

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
    public int GetCurrentPhase()
    {
        return currentPhase;
    }

}
