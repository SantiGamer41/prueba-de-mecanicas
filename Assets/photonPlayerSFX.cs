using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class photonPlayerSFX : MonoBehaviour
{

    public AudioSource remateSource;
    public AudioClip[] rematesSFX;

    PhotonView viewSFX;


    // Start is called before the first frame update
    void Start()
    {
        viewSFX = GetComponent<PhotonView>();


        int nroR = Random.Range(0, 2);

        GameObject gameSFX = GameObject.Find("GAMESFX");

        if (gameSFX != null)
        {
            remateSource = gameSFX.GetComponent<AudioSource>();
        }
    }
    




    void Update()
    {
        if (viewSFX.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayRemate();
            }
        }
    }

    public void PlayRemate()
    {
        int nroR = Random.Range(0, rematesSFX.Length);
        viewSFX.RPC("PlayRemate1RPC", RpcTarget.All, nroR);
    }

    [PunRPC]
    public void PlayRemate1RPC(int nroR)
    {
        remateSource.clip = rematesSFX[nroR];
        remateSource.Play();
    }
}
