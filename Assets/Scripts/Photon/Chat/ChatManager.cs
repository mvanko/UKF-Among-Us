using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour, IChatClientListener, IOnEventCallback
{
    public const byte SendMessageToAllPlayers = 1;

    [SerializeField] Text toSendText;
    [SerializeField] UnityEngine.UI.Button sendButton;
    [SerializeField] GameObject messageObject;
    [SerializeField] Transform contentObject;

    ChatClient chatClient;

    private string userID;

    private Text textChild;

    public void DebugReturn(DebugLevel level, string message)
    {
        return;
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnConnected()
    {
        
    }

    public void OnDisconnected()
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        userID = PhotonNetwork.LocalPlayer.NickName;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new Photon.Chat.AuthenticationValues(userID));
        sendButton.onClick.AddListener(GetChatMessage);
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
        sendButton.onClick.RemoveListener(GetChatMessage);
    }

    // Update is called once per frame
    void Update()
    {
        chatClient.Service();
    }

    private void GetChatMessage()
    {
        string textík = toSendText.text;
        toSendText.text = "";
        SendToAll(textík, userID);

    }

    private void SendToAll(string message, string playerName)
    {
        string content = playerName+": " + message;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SendMessageToAllPlayers, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SendMessageToAllPlayers)
        {
            if (contentObject != null)
            {
                string messageToShow = (string)photonEvent.CustomData;
                textChild = messageObject.GetComponentInChildren<Text>();
                textChild.text = messageToShow;
                GameObject showMessageObject = Instantiate(messageObject);
                showMessageObject.transform.SetParent(contentObject);
                showMessageObject.transform.localScale = new Vector3(1,1,1);
            }
        }
    }
}
