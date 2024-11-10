using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DisplayPuntosScript : MonoBehaviour
{
    PhotonView viewTxtTurno;
    public int turno;
    public Text txt_Turno;

    // Start is called before the first frame update
    void Start()
    {
        viewTxtTurno = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SumarTurnoDisplay()
    {
        viewTxtTurno.RPC("SumarTurnoDisplayRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SumarTurnoDisplayRPC()
    {
        turno++;
        txt_Turno.text = "Turno " + turno.ToString();
        
    }
}
