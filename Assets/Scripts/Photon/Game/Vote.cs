using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Vote : MonoBehaviour
{
    [SerializeField] Text playerNameText;

    private Button _voteButton;
    private VotingManager _votingManager;

    private int _actorNumber;

    public int ActorNumber
    {
        get { return _actorNumber; }
    }

    private void Awake()
    {
        _voteButton = GetComponent<Button>();
        _actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        _voteButton.onClick.AddListener(OnVotePressed);
    }

    private void OnDestroy()
    {
        _voteButton.onClick.RemoveListener(OnVotePressed);
    }

    public void OnVotePressed()
    {
        _votingManager.CastVote(_actorNumber);
    }

    public void Initialize(Photon.Realtime.Player player, VotingManager votingManager)
    {
        _actorNumber = player.ActorNumber;
        playerNameText.text = player.NickName;
        _votingManager = votingManager;
    }

    public void ToggleButton(bool isInteractible)
    {
        _voteButton.interactable = isInteractible;

    }
}
