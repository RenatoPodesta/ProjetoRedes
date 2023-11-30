using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField]
    public string creatorNickname;

    PhotonView photonView;

    [SerializeField]
    float speed = 100f;

    Rigidbody2D rb;

    float bulletTimeLife = 5f;
    float bulletTimeLifeCount = 0f;

    [SerializeField] string creatorID;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        //creatorNickname = photonView.Owner.NickName;
        //Debug.Log("Nome:" + creatorNickname);
        
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * speed);
    }

    // Update is called once per frame
    void Update()
    {
        LifeManager();
    }

    void LifeManager()
    {
        if (bulletTimeLifeCount >= bulletTimeLife)
        {
            Destroy(this.gameObject);
        }
        else
        {
            bulletTimeLifeCount += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.GetComponent<PlayerController>().photonView.Owner.NickName + "  " + this.photonView);
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController playerCollided = collision.gameObject.GetComponent<PlayerController>();

           if(playerCollided == null) {
                return;
            }
            if (playerCollided.photonView && playerCollided.photonView.Owner.NickName != this.photonView.Owner.NickName)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                //playerCollided.TakeDamage(25f, creatorID);
                PhotonView.Destroy(this.gameObject);
            }
        }
    }

    public void UpdateBullet(string nickname, string playerID)
    {
        creatorNickname = nickname;
        creatorID = playerID;
    }
}
