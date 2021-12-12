using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportHUD : MonoBehaviour
{
    [SerializeField] private Button chatButton;
    [SerializeField] private Text votingTimeText;
    [SerializeField] private GameObject chatPanel;

    private bool chatVisible = false;
    private float votingTime = 101f;

    private void Awake()
    {
        chatButton.onClick.AddListener(ShowChat);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        votingTime -= Time.deltaTime;
        votingTimeText.text = ((int)votingTime).ToString();
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
