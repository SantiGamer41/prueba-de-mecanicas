using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pun2Manager : MonoBehaviour
{
    public GameObject punPruebaPlayer;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(punPruebaPlayer.name, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
