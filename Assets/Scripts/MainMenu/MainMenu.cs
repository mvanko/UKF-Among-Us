using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _uiSettings;
    [SerializeField] private GameObject _uiMultiplayer;
    [SerializeField] private GameObject _uiHowToPlay;

    [SerializeField] private Button _uiMultiplayerButton;
    [SerializeField] private Button _uiSettingsButton;
    [SerializeField] private Button _uiQuitButton;
    [SerializeField] private Button _uiHowToPlayButton;

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
        _uiMultiplayerButton.onClick.AddListener(Multiplayer);
        _uiSettingsButton.onClick.AddListener(OpenSettings);
        _uiQuitButton.onClick.AddListener(QuitGame);
        _uiHowToPlayButton.onClick.AddListener(OpenHowToPlay);
    }

    private void OnDisable()
    {
        _uiMultiplayerButton.onClick.RemoveListener(Multiplayer);
        _uiSettingsButton.onClick.RemoveListener(OpenSettings);
        _uiQuitButton.onClick.RemoveListener(QuitGame);
        _uiHowToPlayButton.onClick.RemoveListener(OpenHowToPlay);
    }

    private void Multiplayer()
    {
        _uiMultiplayer.SetActive(true);
    }

    private void OpenSettings()
    {
        _uiSettings.SetActive(true);
    }

    private void OpenHowToPlay()
    {
        _uiHowToPlay.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
