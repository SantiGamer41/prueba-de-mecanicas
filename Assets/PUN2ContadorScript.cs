using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PUN2ContadorScript : MonoBehaviour
{
    public int contador;
    public Text contadorDisplay;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ContadorSuma()
    {
        view.RPC("ContadorSumaRPC", RpcTarget.All);
    }
    
    [PunRPC]
    void ContadorSumaRPC()
    {
        contador++;
        contadorDisplay.text = contador.ToString();
        Debug.LogError(contador);
    }
}
