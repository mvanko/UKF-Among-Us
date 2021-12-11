using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Button killButton;
    [SerializeField] private Button reportButton;
    [SerializeField] private Button useButton;

    void Awake()
    {
        Player.OnPlayerReady += PlayerReady;
    }

    private void OnDestroy()
    {
        Player.OnPlayerReady -= PlayerReady;
        Player.LocalPlayer.OnKillAvailable -= UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable -= UpdateReportButton;
        Player.LocalPlayer.OnUseAvailable -= UpdateUseButton;
        useButton.onClick.RemoveListener(MakeInteraction);
    }

    private void PlayerReady()
    {
        Debug.LogError("DONE: " + Player.LocalPlayer.IsImposter);
        killButton.interactable = Player.LocalPlayer.KillAvailable;
        reportButton.interactable = Player.LocalPlayer.ReportAvailable;
        useButton.interactable = Player.LocalPlayer.UseAvailable;
        killButton.gameObject.SetActive(Player.LocalPlayer.IsImposter);
        useButton.gameObject.SetActive(!Player.LocalPlayer.IsImposter);

        useButton.onClick.AddListener(MakeInteraction);

        Player.LocalPlayer.OnKillAvailable += UpdateKillButton;
        Player.LocalPlayer.OnReportAvailable += UpdateReportButton;
        Player.LocalPlayer.OnUseAvailable += UpdateUseButton;
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
