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
using UnityEngine.TextCore.Text;

public class ChooseCharacter : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] public CharacterType characterType;

    Color corPadrao;

    [SerializeField] public TextMeshProUGUI Nname;

    bool canBeChoosed = true;
    [SerializeField] public string playerName = "";
    [SerializeField] int playerID = 0;

    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canBeChoosed == false)
        {
            return;
        }

        object playerNameReturn;
        object playerIDReturn;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Name", out playerNameReturn);
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerOrder", out playerIDReturn);

        CleanSelectionInParent((string)playerNameReturn);

        Hashtable playerPropTemp = new Hashtable();
        playerPropTemp.Add("characterType", characterType);
        PhotonNetwork.SetPlayerCustomProperties(playerPropTemp);

        GetComponent<PhotonView>().RPC("ChooseYCharacter", RpcTarget.AllBuffered, (string)playerNameReturn, (int)playerIDReturn);
    }

    public void CleanSelectionInParent(string pName)
    {
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
         //   CleanSelectionInParentRPC(pName);
            //GetComponent<PhotonView>().RPC("CleanSelectionInParentRPC", RpcTarget.OthersBuffered, (string)pName);

       //}
        //else
       // {
            GetComponent<PhotonView>().RPC("CleanSelectionInParentRPC", RpcTarget.AllBuffered, (string)pName);
       // }

    }

    [PunRPC]
    public void CleanSelectionInParentRPC(string pName)
    {
        GetComponentInParent<CharacterElementHandler>().CleanSelection(characterType, pName);
    }

    [PunRPC]
    public void ChooseYCharacter(string pName, int order)
    {
        Debug.Log(characterType);
        GetComponent<Image>().color = new Color(100, 100, 100, 100);
        Nname.color = GetPlayerColor(order);
        Nname.text = pName;
        playerName = pName;
        canBeChoosed = false;


        GetComponentInParent<UI_Gameplay>().PlayersReady();
    }

    public void ResetData(bool notFirst = true)
    {
        canBeChoosed = true;
        playerName = "";
        playerID = 0;

        GetComponent<Image>().color = corPadrao;
        Nname.text = "";
        Nname.color = new Color(255, 255, 255);

        if (notFirst)
        GetComponentInParent<UI_Gameplay>().PlayersClean();

    }

    

    void Start()
    {
        corPadrao = GetComponent<Image>().color;
        ResetData(false);
    }

    void Update()
    {
        
    }

    public Color GetPlayerColor(int playerOrder)
    {
        switch (playerOrder)
        {
            case 1:
                return Color.gray;

            case 2:
                return Color.magenta;
            
            case 3:
                return Color.white;

            default:
                return Color.black;
        }
    }

}

public enum CharacterType { red = 0, green = 1, yellow = 2};
