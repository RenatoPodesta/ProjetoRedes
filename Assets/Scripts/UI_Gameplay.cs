using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class UI_Gameplay : MonoBehaviour
{
    [SerializeField]
    GameObject ui_Buttons;

    public int playersCount = 0;
    public int playersReady = 0;

    private void Start()
    {
        ui_Buttons.SetActive(false);
    }

    public void PlayersReady()
    {
        
        playersReady++;

        Debug.Log(playersReady + " " + playersCount);

        if (playersCount == playersReady && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            ui_Buttons.SetActive(true);
        }
    }

    public void PlayersClean()
    {
        if (playersCount == playersReady && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            ui_Buttons.SetActive(false);
        }
        playersReady--;
    }

    [PunRPC]
    public void NewPlayer(int newCount)
    {
        Debug.Log("NewPlayer1");
        Debug.Log(newCount);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("IVan2");
            ui_Buttons.SetActive(false);
        }
        
        playersCount = newCount;
    }

    public void NewPlayerRPC(int newCount)
    {
        GetComponent<PhotonView>().RPC("NewPlayer", RpcTarget.AllBuffered, newCount);
    }
}
