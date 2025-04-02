using UnityEngine;
using UnityEngine.Rendering;

public enum TaskType
{
    Plant,
    TimeJump
}
[CreateAssetMenu(fileName = "TodoData", menuName = "Scriptable Objects/TodoData")]
public class TodoData : ScriptableObject
{
    public string id;
    public TaskType type;
    public int targetCount;
    public string description;
    public string innerMonologue;
    //[HideInInspector]
    public bool isCompleted = false;
}
