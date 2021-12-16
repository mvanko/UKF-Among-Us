
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private Image progressBar;
    [SerializeField] private Button killButton;
    [SerializeField] private Button reportButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button leaveButton;

    [SerializeField] private Text killCooldownText;
    [SerializeField] private Text impostersWonText;
    [SerializeField] private Text crewmatesWonText;
    [SerializeField] private Text locationText;

    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject reportPanel;
    [SerializeField] private GameObject wonPanel;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas reportCanvas;

    public static HUD Instance;

    void Awake()
    {
        Instance = this;
        Player.OnPlayerReady += RegisterPlayerCallbacks;
        GameManager.OnGameManagerReady += RegisterGameManagerCallbacks;
    }

    private void Start()
    {
        continueButton.onClick.AddListener(ContinueGame);
        leaveButton.onClick.AddListener(LeaveGame);
        useButton.onClick.AddListener(MakeInteraction);
        settingButton.onClick.AddListener(OpenSettings);
    }

    private void OnDestroy()
    {
        Player.OnPlayerReady -= RegisterPlayerCallbacks;
        if (Player.LocalPlayer != null)
        {
            Player.LocalPlayer.OnPlayerUpdated -= UpdateGameUI;
            Player.LocalPlayer.OnKillAvailable -= UpdateKillButton;
            Player.LocalPlayer.OnReportAvailable -= UpdateReportButton;
            Player.LocalPlayer.OnUseAvailable -= UpdateUseButton;
        }

        GameManager.OnGameManagerReady -= RegisterGameManagerCallbacks;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMinigameAdded -= (minigame) => UpdateProgressBar();
            GameManager.Instance.OnMinigameRemoved -= (minigame) => UpdateProgressBar();
            GameManager.Instance.OnTasksUpdated -= UpdateProgressBar;
            GameManager.Instance.CrewmatesWon -= CrewmatesWon;
            GameManager.Instance.ImpostersWon -= ImpostersWon;
        }

        continueButton.onClick.RemoveListener(ContinueGame);
        leaveButton.onClick.RemoveListener(LeaveGame);
        useButton.onClick.RemoveListener(MakeInteraction);
        settingButton.onClick.RemoveListener(OpenSettings);
    }

    private void RegisterGameManagerCallbacks() 
    {
        GameManager.Instance.OnTasksUpdated += UpdateProgressBar;
        GameManager.Instance.CrewmatesWon += CrewmatesWon;
        GameManager.Instance.ImpostersWon += ImpostersWon;
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

    private void OpenSettings()
    {
        if (settingPanel.active == false)
        {
            settingPanel.SetActive(true);
        }
        else
        {
            settingPanel.SetActive(false);
        }
    }

    private void ShowLocation(string mapLocation)
    {
        string location = mapLocation;
        locationText.gameObject.SetActive(true);
        locationText.text = location;
        StartCoroutine(LocationTimer());
    }

    private void OnEnable()
    {
        MapLocations.showMapLocation += ShowLocation;
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        MapLocations.showMapLocation -= ShowLocation;
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void BodyFound()
    {
        reportPanel.gameObject.SetActive(true);
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

    private void ContinueGame()
    {
        settingPanel.SetActive(false);
    }

    private void LeaveGame()
    {
        GameManager.Instance.Destroy();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        base.OnLeftRoom();
    }

    private void CrewmatesWon()
    {
        wonPanel.SetActive(true);
        crewmatesWonText.gameObject.SetActive(true);
        StartCoroutine(LoadMainMenu());
    }

    private void ImpostersWon()
    {
        wonPanel.SetActive(true);
        impostersWonText.gameObject.SetActive(true);
        StartCoroutine(LoadMainMenu());
    }

    public void HideReportHUD()
    {
        reportCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
    }


    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);
        reportPanel.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        reportCanvas.gameObject.SetActive(true);
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(5);
        LeaveGame();
    }

    IEnumerator LocationTimer()
    {
        yield return new WaitForSeconds(1);
        locationText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Player.LocalPlayer != null)
        {
            if (Player.LocalPlayer.KillCooldown > 0f)
            {
                killCooldownText.text = $"{(int)Player.LocalPlayer.KillCooldown}";
            }
            else if (killCooldownText.gameObject.activeSelf)
            {
                killCooldownText.gameObject.SetActive(false);
            }
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 2)
        {
            BodyFound();
        }
    }
}
