using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SocialPlatforms;

public class BtnBack : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] string sceneName = "SampleScene";

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBack);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBack);
    }

    void OnBack()
    {
        GameObject netCtrl = GameObject.Find("NetworkController");
        Destroy(netCtrl);
        ResetScore();
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScore()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }

        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("Score", 0);

        Debug.Log("Resetou");
        PhotonNetwork.PlayerList[0].SetCustomProperties(playerProperties);
        PhotonNetwork.PlayerList[1].SetCustomProperties(playerProperties);
    }
}
