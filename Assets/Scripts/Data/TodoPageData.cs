using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TodoPageData", menuName = "Scriptable Objects/TodoPageData")]
public class TodoPageData : ScriptableObject
{
    public List<TodoData> todoList;
    public int phase;
}
