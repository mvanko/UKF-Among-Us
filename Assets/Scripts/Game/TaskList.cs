using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    [SerializeField] private Transform _taskItemContent;
    [SerializeField] private TaskItem _taskItemPrototype;

    private void Awake()
    {
        GameManager.Instance.OnMinigameAdded += AddMinigame;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMinigameAdded -= AddMinigame;
    }

    private void AddMinigame(Minigame minigame)
    {
        Instantiate(_taskItemPrototype, _taskItemContent).Setup(minigame);
    }
}
