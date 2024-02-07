using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviourPunCallbacks
{
    
    public static GameManeger instance;

    [SerializeField] Transform[] SpawnPoints;

    public List<GameObject> ListaJugadores;
    public PhotonView PV;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room. HI! " + newPlayer.NickName);

        PV.RPC("RPC_ListaJugadores", RpcTarget.AllViaServer);

        // ListContent.transform.localScale = Vector3.one;
        //PlayerUser.GetComponent<PlayerListInicializer>().initialize(newPlayer.NickName, true, newPlayer.ActorNumber);

        // StartGameBtn.gameObject.SetActive(checkPlayersReady());
    }
}
