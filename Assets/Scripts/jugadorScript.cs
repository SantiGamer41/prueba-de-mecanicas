using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class jugadorScript : MonoBehaviour
{
    DisplayPuntosScript displayPuntosScript;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        displayPuntosScript = FindObjectOfType<DisplayPuntosScript>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (view.IsMine)
        {
           if (Input.GetKeyDown(KeyCode.Space))
            {
                displayPuntosScript.SumarTurnoDisplay();
            }
          
        }
        */
    }
}
