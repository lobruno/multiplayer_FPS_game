using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
	

	GameObject controller;

	int kills;
	int deaths;
	int coins;

    PhotonView PV;
    void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	void Start()
	{
		if(PV.IsMine)
		{
			CreateController();
		}
	}

	void CreateController()
	{
		Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
	}

	public void Die()
	{
        Hashtable hash = new Hashtable();
        hash.Add("isDie", 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        PhotonNetwork.Destroy(controller);
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(0);
        MenuManager.Instance.OpenMenu("loading");
       

        //deaths++;

        //Hashtable hash = new Hashtable();
        //hash.Add("deaths", deaths);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void GetKill()
	{
		PV.RPC(nameof(RPC_GetKill), PV.Owner);
	}

	[PunRPC]
	void RPC_GetKill()
	{
		kills++;

		Hashtable hash = new Hashtable();
		hash.Add("kills", kills);
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
	}

    public void GetCoin()
    {
        PV.RPC(nameof(RPC_GetCoin), PV.Owner);
    }

    [PunRPC]
    void RPC_GetCoin()
    {
        coins++;

        Hashtable hash = new Hashtable();
        hash.Add("coins", coins);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
	{
		return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
	}
}