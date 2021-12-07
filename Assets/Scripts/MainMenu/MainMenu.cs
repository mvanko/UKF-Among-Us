using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _uiSettings;
    [SerializeField] private GameObject _uiMultiplayer;

    [SerializeField] private Button _uiStartButton;
    [SerializeField] private Button _uiMultiplayerButton;
    [SerializeField] private Button _uiSettingsButton;
    [SerializeField] private Button _uiQuitButton;

    private const string WAITINGROOM = "Waiting Room";

    private void Awake()
    {
        if(PlayerPrefs.HasKey("fullScreenMode"))
        {
            Screen.SetResolution(PlayerPrefs.GetInt("width"),
                PlayerPrefs.GetInt("height"), (FullScreenMode) PlayerPrefs.GetInt("fullScreenMode"));
        }
    }

    private void OnEnable()
    {
        _uiStartButton.onClick.AddListener(StartGame);
        _uiMultiplayerButton.onClick.AddListener(Multiplayer);
        _uiSettingsButton.onClick.AddListener(OpenSettings);
        _uiQuitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        _uiStartButton.onClick.RemoveListener(StartGame);
        _uiMultiplayerButton.onClick.RemoveListener(Multiplayer);
        _uiSettingsButton.onClick.RemoveListener(OpenSettings);
        _uiQuitButton.onClick.RemoveListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(WAITINGROOM);
    }

    private void Multiplayer()
    {
        _uiMultiplayer.SetActive(true);
    }

    private void OpenSettings()
    {
        _uiSettings.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
