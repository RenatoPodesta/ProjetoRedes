using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Corner : MonoBehaviour
{
    Transform tr;

    Collider[] colisor;

    [SerializeField]
    GameObject ball;

    [SerializeField]
    GameObject cornerRB;

    Vector3 startPositionBall = new Vector3(0, 5, 0);
    Quaternion startRotationBall = new Quaternion(0, 0, 0, 0);
    void Start()
    {
        tr = transform;
    }

    private void Update()
    {
        ColliderCorner();
    }

    void ColliderCorner()
    {
        colisor = Physics.OverlapBox(tr.position, transform.localScale / 2, tr.rotation);

        foreach (Collider itemColisor in colisor)
        {
            if (itemColisor.gameObject.tag == "Ball")
            {

                PhotonNetwork.Destroy(itemColisor.gameObject);
                PositionBall();
            }
        }
    }

    public void PositionBall()
    {
        if (cornerRB.gameObject.tag == "Corner")
        {
            PhotonNetwork.Instantiate(ball.name, ball.transform.position, ball.transform.rotation);
            ball.transform.SetPositionAndRotation(startPositionBall, startRotationBall);

        }
    }
}
