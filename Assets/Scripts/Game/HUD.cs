
using System;
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
    [SerializeField] private Button settingButton;

    [SerializeField] private Text bodyReportedText;
    [SerializeField] private Text killCooldownText;

    [SerializeField] private Canvas settingCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas reportCanvas;


    void Awake()
    {
        Player.OnPlayerReady += RegisterPlayerCallbacks;
        GameManager.OnGameManagerReady += RegisterGameManagerCallbacks;
    }

    private void Start()
    {
        useButton.onClick.AddListener(MakeInteraction);
        settingButton.onClick.AddListener(OpenSettings);
    }

    private void OnDestroy()
    {
        Player.OnPlayerReady -= RegisterPlayerCallbacks;
        Player.LocalPlayer.OnReportStarted -= BodyFound;
        Player.LocalPlayer.OnPlayerUpdated -= UpdateGameUI;
        Player.LocalPlayer.OnKillAvailable -= UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable -= UpdateReportButton;
        Player.LocalPlayer.OnUseAvailable -= UpdateUseButton;

        GameManager.OnGameManagerReady -= RegisterGameManagerCallbacks;

        GameManager.Instance.OnMinigameAdded -= (minigame) => UpdateProgressBar();
        GameManager.Instance.OnMinigameRemoved -= (minigame) => UpdateProgressBar();
        GameManager.Instance.OnTasksUpdated -= UpdateProgressBar;

        useButton.onClick.RemoveListener(MakeInteraction);
        settingButton.onClick.RemoveListener(OpenSettings);
    }

    private void RegisterGameManagerCallbacks() 
    {
        GameManager.Instance.OnTasksUpdated += UpdateProgressBar;
        GameManager.Instance.OnMinigameAdded += (minigame) => UpdateProgressBar();
        GameManager.Instance.OnMinigameRemoved += (minigame) => UpdateProgressBar();
    }

    private void RegisterPlayerCallbacks()
    {
        Player.LocalPlayer.OnPlayerUpdated += UpdateGameUI;
        Player.LocalPlayer.OnKillAvailable += UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable += UpdateReportButton;
        Player.LocalPlayer.OnReportStarted += BodyFound;
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

    private void OpenSettings()
    {
        if (settingCanvas.enabled == false)
        {
            settingCanvas.enabled = true;
        }
        else
        {
            settingCanvas.enabled = false;
        }
       
    }

    public void BodyFound()
    {
        bodyReportedText.gameObject.SetActive(true);
        StartCoroutine(DelayCoroutine());

    }

    private void UpdateKillButton(bool value)
    {
        killButton.interactable = value;
        killCooldownText.gameObject.SetActive(Player.LocalPlayer.KillCooldown > 0f);
    }

    private void UpdateReportButton(bool value)
    {
        reportButton.interactable = value;
    }

    private void UpdateUseButton(bool value)
    {
        useButton.interactable = value;
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);

        gameCanvas.gameObject.SetActive(false);
        reportCanvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Player.LocalPlayer.KillCooldown > 0f)
        {
            killCooldownText.text = $"{(int) Player.LocalPlayer.KillCooldown}";
        }
        else if(killCooldownText.gameObject.activeSelf)
        {
            killCooldownText.gameObject.SetActive(false);
        }
    }
}
