using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class marcadorScript : MonoBehaviour
{
    PhotonView viewMarcador;
    public GameObject[] MarcadorLeft;
    public GameObject[] MarcadorRight;

    public int puntosLeft = 1;
    public int puntosRight = 1;

    // Start is called before the first frame update
    void Start()
    {
        viewMarcador = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SumarMarcadorLeft();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SumarMarcadorRight();
        }
    }

    public void SumarMarcadorLeft()
    {
        viewMarcador.RPC("SumarMarcadorLeft_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void SumarMarcadorLeft_RPC()
    {
        if (puntosLeft < MarcadorLeft.Length)
        {
            MarcadorLeft[puntosLeft].SetActive(true);

            if (puntosLeft > 0)
            {
                MarcadorLeft[puntosLeft - 1].SetActive(false);
            }

            puntosLeft++;
        }
    }

    public void SumarMarcadorRight()
    {
        viewMarcador.RPC("SumarMarcadorRight_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void SumarMarcadorRight_RPC()
    {
        if (puntosRight < MarcadorRight.Length)
        {
            MarcadorRight[puntosRight].SetActive(true);

            if (puntosRight > 0)
            {
                MarcadorRight[puntosRight - 1].SetActive(false);
            }

            puntosRight++; // Aumenta el puntaje de la derecha
        }
    }

}
