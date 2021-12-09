using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Button killButton;
    [SerializeField] private Button reportButton;

    void Start()
    {
        Player.OnPlayerReady += PlayerReady;
    }

    private void OnDestroy()
    {
        Player.OnPlayerReady -= PlayerReady;
        Player.LocalPlayer.OnKillAvailable -= UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable -= UpdateReportButton;
    }

    private void PlayerReady()
    {
        killButton.interactable = Player.LocalPlayer.KillAvailable;
        reportButton.interactable = Player.LocalPlayer.ReportAvailable;
        killButton.gameObject.SetActive(Player.LocalPlayer.IsImposter);

        Player.LocalPlayer.OnKillAvailable += UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable += UpdateReportButton;
    }

    private void UpdateKillButton(bool value)
    {
        killButton.interactable = value;
    }

    private void UpdateReportButton(bool value)
    {
        reportButton.interactable = value;
    }
}
