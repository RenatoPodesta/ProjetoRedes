using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System.ComponentModel;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;
using Photon.Pun.Demo.PunBasics;

public class Goal : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] Canvas cvScore;
    [SerializeField] Canvas hud;

    [Header("Txt")]
    [SerializeField] TextMeshProUGUI winnerName;
    
    [HideInInspector]
    public TMP_InputField initialName;
    
    [SerializeField] TMP_Text scoreTxtHome;
    [SerializeField] TMP_Text scoreTxtAway;


    [Header("Game Objects")]
    [SerializeField] GameObject goal;
    [SerializeField] GameObject ball;

    int score = 0;
    public int totalPoints = 3;

    [SerializeField]
    Collider[] colisor;

    // distancia para o gol
    float minDistancia;

    Transform tr;

    // posição inicial da bola
    Vector3 startPositionBall = new Vector3(0, 5, -0.5f);
    Quaternion startRotationBall = new Quaternion(0, 0, 0, 0);

    // posição inicial da bola
    Quaternion startRotationPlayer = new Quaternion(0, 0, 0, 0);

    private void Start()
    {
        tr = transform;
        //PhotonNetwork.PlayerList[0].CustomProperties.;

    }

    private void Update()
    {
        ColliderGoal();
    }

    void ColliderGoal()
    {
        colisor = Physics.OverlapBox(tr.position, transform.localScale / 2, tr.rotation);

        if (goal.gameObject.tag == "Home")
        {
            foreach (Collider itemColisor in colisor)
            {
                if (itemColisor.gameObject.tag == "Ball")
                {
                    GetComponent<PhotonView>().RPC("UpdateBoard", RpcTarget.All, false);
                    PhotonNetwork.Destroy(itemColisor.gameObject);
                    PositionBall();
                    GameFlow.Goal();
                    CountGoalAway();
                }
            }
        } else if (goal.gameObject.tag == "Away")
        {
            foreach (Collider itemColisor in colisor)
            {
                if (itemColisor.gameObject.tag == "Ball")
                {
                    GetComponent<PhotonView>().RPC("UpdateBoard", RpcTarget.All, true);
                    PhotonNetwork.Destroy(itemColisor.gameObject);
                    PositionBall();
                    GameFlow.Goal();
                    CountGoalHome();
                }
            }
        }
    }

    [PunRPC]
    void UpdateBoard(bool isHome)
    {
        Debug.Log("Placar atualizado " + isHome);
        score += 1;
        if(isHome)
        scoreTxtHome.text = score.ToString();
        else scoreTxtAway.text = score.ToString();
        CheckScore();
    }

    [PunRPC]
    void CountGoalHomeRPC()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("Master fez gol");
            UpdatePlayerScore(true);
        }
    }

    [PunRPC]
    void CountGoalAwayRPC()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("Client fez gol");
            UpdatePlayerScore(false);
        }
        
    }

    void CountGoalHome()
    {
        Debug.LogWarning("CountGoalHome");
        GetComponent<PhotonView>().RPC("CountGoalHomeRPC", RpcTarget.AllBuffered);
    }

    void CountGoalAway()
    {
        Debug.LogWarning("CountGoalAway");
        GetComponent<PhotonView>().RPC("CountGoalAwayRPC", RpcTarget.AllBuffered);
    }

    void CheckScore()
    {
        if (score >= totalPoints)
        {
            score = totalPoints;
            Time.timeScale = 0;
            ScoreCanvas();
        }
    }

    void UpdatePlayerScore(bool isHome)
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }
        int playerID = isHome ? 0 : 1;

        object playerScore;

        PhotonNetwork.PlayerList[playerID].CustomProperties.TryGetValue("Score", out playerScore);

        Debug.Log("OldScore" + playerScore);
        int newScore = (int)playerScore;
        newScore++; 
        Debug.Log("NewScore" + newScore);
        
        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("Score", newScore);
        
        PhotonNetwork.PlayerList[playerID].SetCustomProperties(playerProperties);

    }

    

    void ScoreCanvas()
    {
        cvScore.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        if (PhotonNetwork.LocalPlayer.IsMasterClient && !PhotonNetwork.LocalPlayer.IsMasterClient) { PhotonNetwork.Disconnect(); }
        CompareScore();
    }

    void CompareScore()
    {
        object score0;
        PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("Score", out score0);

        object score1;
        PhotonNetwork.PlayerList[1].CustomProperties.TryGetValue("Score", out score1);

        int newScore0 = (int)score0;
        int newScore1 = (int)score1;

        object playerName0;
        PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("Name", out playerName0);
        Debug.LogWarning(playerName0 + " " + newScore0);

        object playerName1;
        PhotonNetwork.PlayerList[1].CustomProperties.TryGetValue("Name", out playerName1);
        Debug.LogWarning(playerName1 + " " + newScore1);

        if (newScore0 > newScore1)
        {
            winnerName.text = (string)playerName0;

        } else if (newScore0 < newScore1)
        {
            winnerName.text = (string)playerName1;
        }
    }

    public void PositionBall()
    {
        if (goal.gameObject.tag == "Home" || goal.gameObject.tag == "Away")
        {
            PhotonNetwork.Instantiate(ball.name, ball.transform.position, ball.transform.rotation);
            //PhotonNetwork.Destroy(ball);
            //GameObject.Instantiate(ball, ball.transform.position, ball.transform.rotation);
            
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.transform.SetPositionAndRotation(startPositionBall, startRotationBall);

        }
    }
}
