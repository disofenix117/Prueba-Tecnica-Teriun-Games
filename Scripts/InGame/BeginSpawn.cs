using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeginSpawn : MonoBehaviour
{
    void Start()
    {

        Spawn();


    }


    void Spawn()
    {
        int i = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        GameObject pyr;
        Debug.Log("valor de tag: " + PhotonNetwork.LocalPlayer.NickName);

        pyr = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerReady"), Vector3.one, Quaternion.identity);
        GameManeger.instance.PV = pyr.GetComponent<PhotonView>();
    }
}
