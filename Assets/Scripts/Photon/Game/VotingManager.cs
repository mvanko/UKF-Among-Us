using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class VotingManager : MonoBehaviour
{
    [SerializeField] Transform[] buttonSpawnPoints;
    [SerializeField] Button skipVoteButton;
    [SerializeField] Text voteText;
    [SerializeField] Vote _voteItemPrefab;
    [SerializeField] GameObject voteOffPanel;

    [HideInInspector] bool HasAlreadyVoted;

    private List<Vote> votePlayerList = new List<Vote>();
    private List<int> playersAlreadyVotedList = new List<int>();
    private List<int> playersVotedOffList = new List<int>();
    private List<int> deadPlayersActorNumbersList = new List<int>();

    private byte ResetPositionAndUnfreeze = 99;

    private PhotonView PV;

    public static VotingManager VotingInstance;

    private void Awake()
    {
        VotingInstance = this;
        PV = GetComponent<PhotonView>();
        PopulatePlayerList();
        playersAlreadyVotedList.Clear();
        playersVotedOffList.Clear();
        HasAlreadyVoted = false;
    }

    private void OnEnable()
    {
        skipVoteButton.onClick.AddListener(SkipVote);
    }

    private void OnDisable()
    {
        skipVoteButton.onClick.RemoveListener(SkipVote);
    }

    private void SkipVote()
    {
        CastVote(-1);
    }

    public void CastVote(int targetActorNumber)
    {
        if (HasAlreadyVoted)
        {
            return;
        }

        HasAlreadyVoted = true;
        DisableAllButtons();        
        PV.RPC("RPCCastPlayerVote", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, targetActorNumber);
    }

    private void PopulatePlayerList()
    {
        deadPlayersActorNumbersList =  GameManager.Instance.GetDeadPlayersActorNumbers();
        for (int i = 0; i<votePlayerList.Count; i++)
        {
            Destroy(votePlayerList[i].gameObject);
        }

        votePlayerList.Clear();

        foreach (KeyValuePair<int, Photon.Realtime.Player> p in PhotonNetwork.CurrentRoom.Players)
        {
            Vote newPlayerVoteButton = Instantiate(_voteItemPrefab, buttonSpawnPoints[p.Value.ActorNumber-1]);
            newPlayerVoteButton.Initialize(p.Value, this);
            newPlayerVoteButton.gameObject.SetActive(true);
            votePlayerList.Add(newPlayerVoteButton);

            if (p.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber || deadPlayersActorNumbersList.Contains(p.Value.ActorNumber))
            {
                newPlayerVoteButton.ToggleButton(false);
            }
        }
    }

    private void DisableAllButtons()
    {
        skipVoteButton.interactable = false;
        foreach(Vote voteButton in votePlayerList)
        {
            voteButton.ToggleButton(false);
        }
    }

    private void SetGameAfterVoting()
    {
        voteOffPanel.gameObject.SetActive(false);
        votePlayerList.Clear();
        HUD.Instance.HideReportHUD();
    }

    [PunRPC]
    public void RPCCastPlayerVote(int localActorNumber, int targetActorNumber)
    {
        int remainingPlayers = PhotonNetwork.CurrentRoom.PlayerCount - GameManager.Instance.GetDeadPlayersCount();

        if (!playersAlreadyVotedList.Contains(localActorNumber))
        {
            playersAlreadyVotedList.Add(localActorNumber);
            playersVotedOffList.Add(targetActorNumber);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (playersAlreadyVotedList.Count < remainingPlayers)
        {
            return;
        }

        Dictionary<int, int> playerVoteCount = new Dictionary<int, int>();

        foreach (int votedPlayer in playersVotedOffList)
        {
            if (!playerVoteCount.ContainsKey(votedPlayer))
            {
                playerVoteCount.Add(votedPlayer, 0);
            }
            playerVoteCount[votedPlayer]++;
        }

        int mostVotedPlayer = -1;
        int mostVotes = int.MinValue;

        foreach (KeyValuePair<int, int> playerVote in playerVoteCount)
        {
            if (playerVote.Value > mostVotes)
            {
                mostVotes = playerVote.Value;
                mostVotedPlayer = playerVote.Key;
            }
        }

        if (mostVotes >= remainingPlayers / 2)
        {
            //TODO respawn a odötartovaù kill count, a zabiù vyhodenÈho
            PV.RPC("RPCKickPlayer", RpcTarget.All, mostVotedPlayer);
        }
    }

    [PunRPC]
    private void RPCKickPlayer(int votedPlayer)
    {
        voteOffPanel.gameObject.SetActive(true);

        string playerName = string.Empty;

        foreach (KeyValuePair<int, Photon.Realtime.Player> p in PhotonNetwork.CurrentRoom.Players)
        {
            if (p.Value.ActorNumber == votedPlayer)
            {
                playerName = p.Value.NickName;
                break;
            }
        }

        if(votedPlayer == -1)
        {
            voteText.text = "No one has been voted off!";
        }
        else
        {
            voteText.text = playerName + " has been voted off!";
            
        }

        int content = votedPlayer;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All};
        PhotonNetwork.RaiseEvent(ResetPositionAndUnfreeze, content, raiseEventOptions, SendOptions.SendReliable);

        StartCoroutine(CloseReportCanvas());
    }

    IEnumerator CloseReportCanvas()
    {
        yield return new WaitForSeconds(5);
        SetGameAfterVoting();
    }
}
