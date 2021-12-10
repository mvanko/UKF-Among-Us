using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "MinigameData", order = 3)]
public class MinigameData : ScriptableObject 
{
    public enum TaskType
    {
        Short,
        Medium,
        Long
    }

    [Header("Names")]
    public string foodTask;
    public string pcTask;
    public string findCourseTask;
    public string tableTask;
    public string receptionTask; 
    public string vestibul1Task;
    public string vestibul2Task;
    public string toiletTask;
    public string bookTask;
    public string aulaTask;
    public string engineRoomTask;
    public string hall1Task;
    public string hall2Task;
}
