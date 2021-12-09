using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    private event Action _callback;
    
    public void Setup(Action OnCloseCallback)
    {
        _callback = OnCloseCallback;
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseMinigame);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseMinigame);
    }

    public void CloseMinigame()
    {
        Destroy(this.gameObject);
        _callback?.Invoke();
    }
}
