using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using System.IO;

public class ReportHUD : MonoBehaviour
{
    [SerializeField] private Button chatButton;
    [SerializeField] private Text votingTimeText;
    [SerializeField] private GameObject chatPanel;

    private bool chatVisible = false;
    private float votingTime = 101f;
    private Scene scene;

    [SerializeField] private GameObject[] buttonSpawns;

    private GameObject _playerButton;

    private void Awake()
    {
        chatButton.onClick.AddListener(ShowChat);
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            buttonSpawns[p.ActorNumber - 1].SetActive(true);
            buttonSpawns[p.ActorNumber - 1].GetComponentInChildren<Text>().text = p.NickName;

        }
    }

    // Update is called once per frame
    void Update()
    {
        votingTime -= Time.deltaTime;
        votingTimeText.text = ((int)votingTime).ToString();

        if(votingTime <= 0)
        {
            votingTime = 0;
            SceneManager.LoadScene(scene.name);
        }
    }

    private void OnDestroy()
    {
        chatButton.onClick.RemoveListener(ShowChat);
    }

    private void ShowChat()
    {
        if (!chatVisible)
        {
            chatPanel.SetActive(true);
            chatVisible = !chatVisible;
        }
        else
        {
            chatPanel.SetActive(false);
            chatVisible = !chatVisible;
        }
        
    }
}
