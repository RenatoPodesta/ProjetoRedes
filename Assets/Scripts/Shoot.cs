using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    // transform do player
    [SerializeField]
    Transform player;

    // transform da bola
    [SerializeField]
    Transform ball;

    // rigidbody da bola
    [SerializeField]
    Rigidbody ballRB;

    // força para aplicar
    [SerializeField]
    float force = 500f;

    //[SerializeField]
    //float verificationRadius = 1;

    // distancia para atacar
    [SerializeField] float minDistancia = 0;
    
    //array de colisor
    Collider[] colisor;

    Transform tr;

    [SerializeField] float shootRate = 1f;

    float shootRateTimer = 0;

    private void Start()
    {
        tr = transform;
    }

    public float ReturnAngle(Vector3 player, Vector3 ball)
    {
        float finalAngle;

        finalAngle = Mathf.Atan2(ball.z - player.z, ball.x - player.x);

        // retornar tangente
        return finalAngle;

        //retornar angulo
        //return finalAngle*180/Mathf.PI;
    }

    private void Update()
    {

        //tecla que retorna a tangente
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReturnAngle(player.position, ball.position);

            Debug.Log(ReturnAngle(player.position, ball.position));
        }

        shootRateTimer += Time.deltaTime;

        if (shootRateTimer > shootRate)
        {
            AddForceInAngleRPC();
        }
    }

    [PunRPC]
    public void AddForceInAngleRPC()
    {
        shootRateTimer = 0;

        colisor = Physics.OverlapSphere(tr.position, minDistancia);
        Collider colisorAtual;

        for (int i = 0; i < colisor.Length; i++)
        {
            colisorAtual = colisor[i];
            if (colisor[i].CompareTag("Ball"))
            {
                Debug.Log("BOLA PERTO");
                float xForce = Mathf.Cos(ReturnAngle(player.position, colisor[i].transform.position)) * force;
                float zForce = Mathf.Sin(ReturnAngle(player.position, colisor[i].transform.position)) * force;

                Vector3 finalForce = new Vector3(xForce, 0, zForce);


                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //ballRB.AddForce(finalForce);
                    colisor[i].gameObject.GetComponent<Rigidbody>().AddForce(finalForce);
                }
                else
                {
                    GetComponent<PhotonView>().RPC("MasterShoot", RpcTarget.MasterClient, finalForce);
                    Debug.Log("Else");
                }
            }
        }
    }

    [PunRPC]
    public void MasterShoot(Vector3 finalForce)
    {
        Debug.Log("MasterShoot");
        GameObject bola = GameObject.FindGameObjectWithTag("Ball");
        bola.GetComponent<Rigidbody>().AddForce(finalForce);

    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDistancia);
        
    }
}
