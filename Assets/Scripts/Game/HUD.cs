using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private Button killButton;
    [SerializeField] private Button reportButton;
    [SerializeField] private Button useButton;

    void Awake()
    {
        Player.OnPlayerReady += RegisterPlayerCallbacks;
        GameManager.OnGameManagerReady += RegisterGameManagerCallbacks;
    }

    private void Start()
    {
        useButton.onClick.AddListener(MakeInteraction);
    }

    private void OnDestroy()
    {
        Player.OnPlayerReady -= RegisterPlayerCallbacks;
        Player.LocalPlayer.OnPlayerUpdated -= UpdateGameUI;
        Player.LocalPlayer.OnKillAvailable -= UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable -= UpdateReportButton;
        Player.LocalPlayer.OnUseAvailable -= UpdateUseButton;

        GameManager.OnGameManagerReady -= RegisterGameManagerCallbacks;

        GameManager.Instance.OnMinigameAdded -= (minigame) => UpdateProgressBar();
        GameManager.Instance.OnMinigameRemoved -= (minigame) => UpdateProgressBar();

        useButton.onClick.RemoveListener(MakeInteraction);
    }

    private void RegisterGameManagerCallbacks() 
    {
        GameManager.Instance.OnMinigameAdded += (minigame) => UpdateProgressBar();
        GameManager.Instance.OnMinigameRemoved += (minigame) => UpdateProgressBar();
    }

    private void RegisterPlayerCallbacks()
    {
        Player.LocalPlayer.OnPlayerUpdated += UpdateGameUI;
        Player.LocalPlayer.OnKillAvailable += UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable += UpdateReportButton;
        Player.LocalPlayer.OnUseAvailable += UpdateUseButton;
        UpdateGameUI();
    }

    private void UpdateGameUI()
    {
        killButton.interactable = Player.LocalPlayer.KillAvailable;
        reportButton.interactable = Player.LocalPlayer.ReportAvailable && !Player.LocalPlayer.IsDead;
        useButton.interactable = Player.LocalPlayer.UseAvailable;
        killButton.gameObject.SetActive(Player.LocalPlayer.IsImposter);
        useButton.gameObject.SetActive(!Player.LocalPlayer.IsImposter);
    }

    private void UpdateProgressBar()
    {
        progressBar.fillAmount = (float) GameManager.Instance.TotalTasksCompleted / GameManager.Instance.TotalTasks;
    }

    private void MakeInteraction()
    {
        Player.LocalPlayer.MakeInteraction(true);
    }

    private void UpdateKillButton(bool value)
    {
        killButton.interactable = value;
    }

    private void UpdateReportButton(bool value)
    {
        reportButton.interactable = value;
    }

    private void UpdateUseButton(bool value)
    {
        useButton.interactable = value;
    }
}
