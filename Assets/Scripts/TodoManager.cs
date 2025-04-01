using System.Collections.Generic;
using UnityEngine;

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

    public int plantCount = 0;
    public int timeJumpCount = 0;

    // todo ÌõÄ¿×´Ì¬
    public List<TodoData> todoList = new List<TodoData>();

    void Start()
    {
        foreach(var task in todoList)
        {
            task.isCompleted = false;
        }
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
    private void CheckTasks(TaskType type, int currentCount)
    {
        foreach (var task in todoList)
        {
            if (task.type == type && !task.isCompleted && currentCount >= task.targetCount)
            {
                task.isCompleted = true;
                Debug.Log($"TODO Complete: {task.description}");
                TodoUI.Instance.MarkTodoComplete(task.id);
            }
        }
    }
}
