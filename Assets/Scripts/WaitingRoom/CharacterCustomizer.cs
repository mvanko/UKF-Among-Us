using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CharacterCustomizer : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] Color[] allColors;
    [SerializeField] Button[] colorButtons;

    public const byte SendColorToAll = 3;
    public const byte EnableColor = 4;

    //colorbutton[0] - button, ktory disabled-nut; colorbutton[1] - button, ktory enabled-nut
    public void SetColor(int colorIndex)
    {
        Color color = Player.LocalPlayer.GetColor();
        int[] buttons = new int[2];
        buttons[0] = colorIndex;
        buttons[1] = ColorToButtonNumber(color);
        Player.LocalPlayer.SetColor(allColors[colorIndex]);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SendColorToAll, buttons, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SendColorToAll)
        {
            int[] data = (int[])photonEvent.CustomData;
            if (data[1] > -1)
            {
                Button buttonToDisable = colorButtons[data[0]];
                Button buttonToEnable = colorButtons[data[1]];
                buttonToDisable.enabled = false;
                buttonToEnable.enabled = true;
            }
            else
            {
                Button buttonToDisable = colorButtons[data[0]];
                buttonToDisable.enabled = false;
            }
            
        }else if (photonEvent.Code == EnableColor)
        {
            Button buttonToEnable = colorButtons[(int)photonEvent.CustomData];
            buttonToEnable.enabled = true;
        }
    }

    private void Awake()
    {
        Player.OnPlayerReady += SetFirstFreeColor;
    }

    private void SetFirstFreeColor()
    {
        int freeColorIndex = GetFirstFreeColor();
        int[] buttons = new int[2];
        buttons[0] = freeColorIndex;
        buttons[1] = -1;
        Player.LocalPlayer.SetColor(allColors[freeColorIndex]);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SendColorToAll, buttons, raiseEventOptions, SendOptions.SendReliable);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnDestroy()
    {
        Player.OnPlayerReady -= SetFirstFreeColor;
    }

    //TODO - aktivovaù farby, ktorÈ boli deselectnutÈ, dokonËi zmenu z RGBA na ËÌslo
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (!newPlayer.IsLocal)
        {
            Color color = Player.LocalPlayer.GetColor();
            int[] buttons = new int[2];
            buttons[0] = ColorToButtonNumber(color);
            buttons[1] = -1;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(SendColorToAll, buttons, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Color color = Player.LocalPlayer.GetColor();
        int colorIndex = ColorToButtonNumber(color);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EnableColor, colorIndex, raiseEventOptions, SendOptions.SendReliable);
    }

    public int GetFirstFreeColor()
    {
        List<int> tempButtons = new List<int> {0,1,2,3,4,5,6,7,8};
        for (int i = 0; i <= colorButtons.Length-1; i++)
        {
            if (colorButtons[i].enabled == false)
            {
                tempButtons.RemoveAt(i);
            }
        }
        int firstFreeIndex = tempButtons[0];
        return firstFreeIndex;
    }

    public int ColorToButtonNumber(Color rgbaColor)
    {
        int number = 0;
        Color color = rgbaColor;
        for (int i = 0; i <= allColors.Length-1; i++)
        {
            if (color == allColors[i])
            {
                number = i;
            }
        }
        return number;
    }
}
