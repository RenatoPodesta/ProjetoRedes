using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon.StructWrapping;
using System.Text;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager instance;

    [Header("Names")]
    [SerializeField] TMP_InputField inputRoomName;
    [SerializeField] public TMP_InputField inputPlayerName;
    [SerializeField] public TextMeshProUGUI ShowPlayerName;


    [Header("UI")]
    [SerializeField] GameObject cvPlayerInput;
    [SerializeField] GameObject cvLobbyInput;
    [SerializeField] GameObject cvConnected;
    [SerializeField] GameObject cvMain;
    [SerializeField] GameObject cvChooseCharacter;


    

    int playerOrder;

    [Header("Scripts")]
    [SerializeField] UI_Gameplay uiGameplay;
    [SerializeField] CharacterElementHandler ceHandler;
    private int characterType;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Teste");
        UiHandler(cvPlayerInput, true);
        UiHandler(cvLobbyInput, false);
        UiHandler(cvConnected, false);
        //PhotonNetwork.ConnectUsingSettings();

    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnGameplayLoaded;
    }




    public void CreateRoom()
    {
        // variavel de nome da room
        string roomName = inputRoomName.text;
        // verificar se existe a room
        if (roomName == ""){
        roomName = ("Room_" + Random.Range(1, 10));
        }
        // cria opcao de numero de player 
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2
        };
        //funcao nativa da photon que entra ou cria uma room, com parametros de nome, opcoes e tipo de lobby
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void ConnectSettings()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = inputPlayerName.text;
        // variavel de nome da room
        string playerName = inputPlayerName.text;
        ShowPlayerName.text = inputPlayerName.text;
        // verificar se existe a room
        if (playerName == "")
        {
            playerName = ("Player_" + Random.Range(1, 99));
        }


        btnLogin();
    }


    void btnLogin()
    {
        UiHandler(cvPlayerInput, false);
        UiHandler(cvLobbyInput, true);
        UiHandler(cvConnected, false);
    }


     void btnLobby()
     {
         UiHandler(cvPlayerInput, false);
         UiHandler(cvLobbyInput, false);
         UiHandler(cvConnected, false);
         UiHandler(cvMain, false);
         UiHandler(cvChooseCharacter, true);
    }


    public override void OnConnected()
    {
        Debug.Log("Conectei");
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectei na Master");
        //PhotonNetwork.JoinLobby();
    }


    public void OnSearch()
    {
        Debug.Log("Entrei na lobby rapido!");
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Conectei na lobby");
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Conectei na room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Tem " + PhotonNetwork.CurrentRoom.PlayerCount + " jogador(es)");
        
        playerOrder = PhotonNetwork.PlayerList.Length;
        Debug.Log("Jogador " + PhotonNetwork.NickName + " é o jogador numero " + playerOrder);

        SetPlayerProperties();
        btnLobby();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        CreateRoom();
    }

    void CreatePlayer()
    {
        object playerType;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("characterType", out playerType);
        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Vector3 startPositionPlayer = new Vector3(-13, (float)1.5, 0);
            Quaternion startRotationPlayer = new Quaternion (0,0,0,1);
            ChangeSkin((CharacterType)playerType, startPositionPlayer, startRotationPlayer);
        }
        else
        {
            Vector3 startPositionPlayer = new Vector3(13, (float)1.5, 0);
            Quaternion startRotationPlayer = new Quaternion(0, 0, 0, 1);
            ChangeSkin((CharacterType)playerType, startPositionPlayer, startRotationPlayer);
        }
    }

    void UiHandler(GameObject ui, bool isActive)
    {
        ui.gameObject.SetActive(isActive);
    }

    void SetPlayerProperties()
    {
        Hashtable playerPropTemp = new Hashtable();

        playerPropTemp.Add("Name", PhotonNetwork.NickName);
        playerPropTemp.Add("Score", 0);
        playerPropTemp.Add("ID", PhotonNetwork.LocalPlayer.UserId);
        playerPropTemp.Add("PlayerOrder", playerOrder);
        

        PhotonNetwork.SetPlayerCustomProperties(playerPropTemp);
        Debug.LogWarning("Propriedades Criadas para " + playerPropTemp["Name"]);

        GetComponent<PhotonView>().RPC("UpdateNumPlayer", RpcTarget.AllBuffered);
        //UpdateNumPlayer();
    }

    [PunRPC]
    public void UpdateNumPlayer()
    {
        Debug.Log("UpdateNumPlayer");
        uiGameplay.NewPlayer(PhotonNetwork.PlayerList.Length);
    }

    [PunRPC]
    public void StartGameplayRPC()
    {
        PhotonNetwork.LoadLevel("Gameplay");
    }

    public void StartGameplay()
    {
        GetComponent<PhotonView>().RPC("StartGameplayRPC", RpcTarget.All);
    }

    void OnGameplayLoaded(Scene cena, LoadSceneMode sceneMode)
    {
        if (cena.name == "Gameplay")
        {
            CreatePlayer();
            Debug.Log("Player Criado");
        }
    }


    public void ChangeSkin(CharacterType characterType, Vector3 startPosition, Quaternion startRotation)
    {
        switch (characterType)
        {
            case (CharacterType.red):
                PhotonNetwork.Instantiate("PlayerRed", startPosition, startRotation);
                break;
            case (CharacterType.green):
                PhotonNetwork.Instantiate("PlayerGreen", startPosition, startRotation);
                break;
            case (CharacterType.yellow):
                PhotonNetwork.Instantiate("PlayerYellow", startPosition, startRotation);
                break;

        }
    }
}
