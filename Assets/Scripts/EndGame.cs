using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class EndGame : MonoBehaviourPunCallbacks
{
    public GameObject endMenu;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text winnerName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            endMenu.SetActive(true);
            //coins.text = "coins: " + PhotonNetwork.PlayerList[0].NickName;
            winnerName.text = "player: " + PhotonNetwork.PlayerList[0].NickName + " win!";

            if (PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("coins", out object coins))
            {
                coinsText.text = "coins: " + coins.ToString();
            }
            else { coinsText.text = "coins: 0" ; }
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int players = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("isDie", out object isDie))
            {
                if (!(isDie.ToString() == "1")) { players++; }
            }
        }
        if (players == 1)
        {
            endMenu.SetActive(true);
            //coins.text = "coins: " + PhotonNetwork.PlayerList[0].NickName;
            winnerName.text = "player: " + PhotonNetwork.PlayerList[0].NickName + " win!";

            if (PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("coins", out object coins))
            {
                coinsText.text = "coins: " + coins.ToString();
            }
        }

        
    }

    

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Destroy(FindObjectOfType<RoomManager>());
        PhotonNetwork.LoadLevel(0);
    }
}
