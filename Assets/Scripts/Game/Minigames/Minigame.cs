using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    private event Action _callbackClose;
    private event Action _callbackWin;

    public void Setup(Action OnCloseCallback, Action OnWinCallback)
    {
        _callbackClose = OnCloseCallback;
        _callbackWin = OnWinCallback;
    }

    internal void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseMinigame);
    }

    internal void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseMinigame);
    }

    internal void CloseMinigame()
    {
        Destroy(this.gameObject);
        _callbackClose?.Invoke();
    }

    internal void WinMinigame()
    {
        Destroy(this.gameObject);
        _callbackWin?.Invoke();
    }
}
