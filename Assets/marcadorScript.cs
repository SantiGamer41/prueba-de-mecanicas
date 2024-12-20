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

    public GameObject panelGanador;
    public Text txt_Ganador;

    // Start is called before the first frame update
    void Start()
    {
        viewMarcador = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

        if(puntosLeft >= 6)
        {
            panelGanador.SetActive(true);
            txt_Ganador.text = ("Ganador: Jugador Izquierda");
        }

        if(puntosRight >= 6)
        {
            panelGanador.SetActive(true);
            txt_Ganador.text = ("Ganador: Jugador Derecha");
        }
    }


    private void ActualizarMarcadorLeft()
    {
        if (puntosLeft < MarcadorLeft.Length)
        {
            MarcadorLeft[puntosLeft].SetActive(true);

            if (puntosLeft > 0)
            {
                MarcadorLeft[puntosLeft - 1].SetActive(false);
            }
        }
        puntosLeft++; 
    }

 
    private void ActualizarMarcadorRight()
    {

        if (puntosRight < MarcadorRight.Length)
        {
            MarcadorRight[puntosRight].SetActive(true);

            if (puntosRight > 0)
            {
                MarcadorRight[puntosRight - 1].SetActive(false);
            }
        }
        puntosRight++;
    }

    public void SumarPuntosLeft()
    {
        viewMarcador.RPC("SumarMarcadorLeft_RPC", RpcTarget.All);
    }

    public void SumarMarcadorRight()
    {
        viewMarcador.RPC("SumarMarcadorRight_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void SumarMarcadorLeft_RPC()
    {
        ActualizarMarcadorLeft();
    }


    [PunRPC]
    public void SumarMarcadorRight_RPC()
    {
        ActualizarMarcadorRight();
    }

}
