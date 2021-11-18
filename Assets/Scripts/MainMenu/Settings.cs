using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button _uiBackButton;

    private void OnEnable()
    {
        _uiBackButton.onClick.AddListener(OpenMainMenu);
    }

    private void OnDisable()
    {
        _uiBackButton.onClick.RemoveListener(OpenMainMenu);
    }

    private void OpenMainMenu()
    {
        this.gameObject.SetActive(false);
    }
}
