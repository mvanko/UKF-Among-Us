using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;

    private Minigame _minigame;

    public void Setup(Minigame minigame)
    {
        _minigame = minigame;
        uiText.text = _minigame.Name();

        GameManager.Instance.OnMinigameRemoved += MarkMinigameAsWon;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMinigameRemoved -= MarkMinigameAsWon;
    }

    private void MarkMinigameAsWon(Minigame minigame)
    {
        if (minigame == _minigame)
        {
            uiText.color = Color.green;
        }
    }
}
