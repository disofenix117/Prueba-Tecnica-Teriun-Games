using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPJ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrando al trigger");
        if (other.CompareTag("Bullet"))
        {
            GameManeger.instance.PV.GetComponent<PlayerMovement>().puntaje += 10;
            GameManeger.instance.PV.RPC("RPC_SumarPuntaje",RpcTarget.AllViaServer,GameManeger.instance.PV.Owner.NickName, GameManeger.instance.PV.GetComponent<PlayerMovement>().puntaje);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
