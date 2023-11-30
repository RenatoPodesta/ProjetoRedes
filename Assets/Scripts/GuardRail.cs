using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GuardRail : MonoBehaviour
{
    Transform tr;

    Collider[] colisor;

    [SerializeField]
    GameObject grailRB;

    [SerializeField] bool isHorizontal;

    [SerializeField]
    GameObject ball;

    [SerializeField]
    float forceGR = 10f;

    Vector3 minDistancia;

    Vector3 startPositionBall = new Vector3(0, 5, 0);
    Quaternion startRotationBall = new Quaternion(0, 0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
    }

    private void Update()
    {
        //ColliderCorner();
    }

    void ColliderCorner()
    {
        colisor = Physics.OverlapBox(tr.position, transform.localScale / 2, tr.rotation);

        foreach (Collider itemColisor in colisor)
        {
            if (itemColisor.gameObject.tag == "Ball")
            {
                
                //PhotonNetwork.Destroy(itemColisor.gameObject);
                //PositionBall();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("Detectou Bola");
            Rigidbody rb;
            rb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 newVelocity = new Vector3(0, 0, 0);

            if (isHorizontal == true)
            {
                Debug.Log("Antes Horizontal " + rb.velocity);
                newVelocity = new Vector3(rb.velocity.x, rb.velocity.y, -rb.velocity.z);
                Debug.Log("Depois Horizontal " + rb.velocity);
            }
            else
            {
                newVelocity = new Vector3(-rb.velocity.x, rb.velocity.y, rb.velocity.z);
                Debug.Log("Vertical " + rb.velocity);

            }

            Debug.Log("RB " + rb.velocity);
            Debug.Log("NEW " + newVelocity);

            rb.velocity = newVelocity;

        }
    }

    //public void PositionBall()
    //{
    //    if (grailRB.gameObject.tag == "Corner")
    //    {
    //        PhotonNetwork.Instantiate(ball.name, ball.transform.position, ball.transform.rotation);
    //        ball.transform.SetPositionAndRotation(startPositionBall, startRotationBall);

    //    }
    //}


    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawCube(transform.position, minDistancia);

    }
}
