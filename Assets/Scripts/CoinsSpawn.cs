using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CoinsSpawn : MonoBehaviourPunCallbacks
{
    public GameObject coin;
    public float minX = -8.5f;
    public float maxX = 8.5f;
    public float minY = -4f;
    public float maxY = 5f;

    public int count = 2;

    PhotonView PV;
    void Awake()
    {
        
        //PV = PhotonNetwork.PlayerList[0].plGetComponent<PhotonView>();
    }

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        for (int i = 0; i < count; i++)
        {
            RPC_Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        PV.RPC("RPC_Spawn", RpcTarget.All);
    }

    //[PunRPC]
    void RPC_Spawn()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Coin"), new Vector2(x, y), Quaternion.identity);
    }
}
