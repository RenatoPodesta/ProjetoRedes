using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 100f;

    float drag;

    Rigidbody rb;

    Vector3 movementPlayer;

    [HideInInspector] public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        
        //SetLifeDefault();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine){
            return;
        }

        movementPlayer = new Vector3(Input.GetAxis("Horizontal"), 0 ,Input.GetAxis("Vertical"));

        //if (Input.GetKeyDown(KeyCode.R)){

        //    newColor = Color.red;
        //    photonView.RPC("changeColor", RpcTarget.AllBuffered, newColor.x, newColor.y, newColor.z, newColor.w);
        //    //changeColor(Color.red);
        //}

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    newColor = Color.green;
        //    photonView.RPC("changeColor", RpcTarget.AllBuffered, newColor.x, newColor.y, newColor.z, newColor.w);
        //    //changeColor(Color.green);
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    photonView.RPC("LifeManager", RpcTarget.AllBuffered, 10f);
        //    //LifeManager(10f);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    photonView.RPC("LifeManager", RpcTarget.AllBuffered, -10f); 
        //    //LifeManager(-10f);
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Shoot();
        //}
    }

    private void FixedUpdate()
    {
        //if (photonView.IsMine)
        MovePlayer(movementPlayer);
        
    }

    void MovePlayer(Vector3 direction)
    {
        rb.velocity = direction * speed * Time.deltaTime;
    }

    //[PunRPC]
    //void changeColor(float r, float g, float b, float a)
    //{
    //    GetComponent<Renderer>().material.color = new Color(r,g,b,a);
    //}
    //void SetLifeDefault()
    //{
    //    life = maxLife;
    //}
    //[PunRPC]
    //void LifeManager(float value)
    //{
    //    life += value;

    //    if(life < 0)
    //        life = 0;

    //    if (life > maxLife)
    //        life = maxLife;

    //    lifeBar.fillAmount = life / maxLife;
    //}
    //void Shoot()
    //{
    //    GameObject temBullet = PhotonNetwork.Instantiate(bullet.name, spawnBulletPoint.position, spawnBulletPoint.rotation);

    //    object myID;
    //    PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("ID", out myID);

    //    temBullet.GetComponent<BulletPlayer>().UpdateBullet(photonView.Owner.NickName, (string)myID);

    //    //temBullet.GetComponent<BulletPlayer>().creatorNickname = photonView.Owner.NickName;


    //}   

    //public void TakeDamage(float value, string enemyBulletID)
    //{
    //       photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, value, enemyBulletID);
    //}

    //[PunRPC]
    //void TakeDamageRPC(float value, string enemyBulletID)
    //{
    //    LifeManager(-value);

    //    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
    //    {
    //        object playerIDReturn;
    //        player.CustomProperties.TryGetValue("ID", out playerIDReturn);
    //        Debug.Log($"O ID dele é " + (string)playerIDReturn);

    //        if (enemyBulletID == (string)playerIDReturn)
    //        {
    //            object playerScoreTemp;
    //            player.CustomProperties.TryGetValue("Score", out playerScoreTemp);
    //            int scoreNew = (int)playerScoreTemp + 1;

    //            Hashtable playerHashTemp = new Hashtable();
    //            playerHashTemp.Add("Score", scoreNew);

    //            player.SetCustomProperties(playerHashTemp);

    //            Debug.Log(scoreNew);
    //        }
    //    }
    //}

    public void RestartPosition()
    {
        GetComponent<PhotonView>().RPC("RestartPositionRPC", RpcTarget.AllBuffered);
        Debug.Log("RestartPosition");
    }

    [PunRPC]
    void RestartPositionRPC()
    {
        Debug.Log("RestartPositionRPC");
        Vector3 startPositionPlayer = new Vector3 (0,0,0);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startPositionPlayer = new Vector3(-12, 1.5f, -0.5f);
        }
        else
        {
            startPositionPlayer = new Vector3(12, 1.5f, -0.5f);
           
        }

        transform.SetPositionAndRotation(startPositionPlayer, transform.rotation);
    }

}



