using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviourPunCallbacks
{

    public static ScoreManager Instance;
    public int player;
    public Text[] nameText;
    public Text[] scoreText;
    public int[] currentPoints;
    public int totalPoints = 3;


    void Start()
    {

        //Check if instance already exists
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        //player = PhotonNetwork.player. - 1;
        GetComponent<PhotonView>().RPC("InitScoreboard", RpcTarget.AllBuffered);
    }



    [PunRPC]
    void InitScoreboard()
    {

        nameText[player].text = "Player " + (player + 1);
        currentPoints[player] = 0;
        totalPoints = 3;
    }


    public void UpdateScore()
    {
        Debug.Log("You got hit");
        currentPoints[player] += 1;
        scoreText[player].text = currentPoints[player].ToString();
        CheckScore();
    }

    void CheckScore()
    {
        if (currentPoints[player] >= totalPoints)
        {
            currentPoints[player] = totalPoints;
            Debug.Log("Game Over " + "player " + player + " has Won");
        }
    }
}