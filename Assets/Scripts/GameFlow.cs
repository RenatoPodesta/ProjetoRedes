using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameFlow : MonoBehaviour
{
    public static void Goal()
    {
        Debug.Log("Goal");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Debug.Log("ForEach");
            player.GetComponent<PlayerController>().RestartPosition();
        }
    }

}
