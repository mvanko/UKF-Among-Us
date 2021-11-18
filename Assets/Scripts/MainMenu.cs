using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _uiMainMenu;
    [SerializeField] private GameObject _uiSettings;

    [SerializeField] private Button _uiStartButton;
    [SerializeField] private Button _uiSettingsButton;
    [SerializeField] private Button _uiQuitButton;

    private const string LEVEL1NAME = "Level1";

    private void Start()
    {
        OpenMainMenu();
    }

    private void OnEnable()
    {
        _uiStartButton.onClick.AddListener(StartGame);
        _uiSettingsButton.onClick.AddListener(OpenSettings);
        _uiQuitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        _uiStartButton.onClick.RemoveListener(StartGame);
        _uiSettingsButton.onClick.RemoveListener(OpenSettings);
        _uiQuitButton.onClick.RemoveListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(LEVEL1NAME);
    }

    private void OpenSettings()
    {
        _uiMainMenu.SetActive(false);
        _uiSettings.SetActive(true);
    }

    private void OpenMainMenu()
    {
        _uiMainMenu.SetActive(true);
        _uiSettings.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
