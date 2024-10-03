using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PUN2FalsoJugadorScript : MonoBehaviour
{
    PUN2ContadorScript contadorScript;
    PhotonView View;

    // Start is called before the first frame update
    void Start()
    {
        View = GetComponent<PhotonView>();
        contadorScript = FindObjectOfType<PUN2ContadorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            contadorScript.ContadorSuma();
        }
    }
}
