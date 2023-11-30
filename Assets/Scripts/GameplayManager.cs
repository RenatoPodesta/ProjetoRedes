using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon.StructWrapping;

public class GameplayManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject myPlayer;
    private void Start()
    {
        CreatePlayer();
    }
    void CreatePlayer()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Vector3 startPositionPlayer = new Vector3(-12, 1.5f, -0.5f);
            PhotonNetwork.Instantiate(myPlayer.name, startPositionPlayer, myPlayer.transform.rotation);
        }
        else
        {
            Vector3 startPositionPlayer = new Vector3(12, 1.5f, -0.5f);
            PhotonNetwork.Instantiate(myPlayer.name, startPositionPlayer, myPlayer.transform.rotation);
        }
    }
}
