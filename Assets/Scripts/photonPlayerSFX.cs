using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class photonPlayerSFX : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip[] rematesSFX;
    public AudioClip[] piquesSFX;
    public AudioClip pitidoSFX;

    PhotonView viewSFX;


    // Start is called before the first frame update
    void Start()
    {
        viewSFX = GetComponent<PhotonView>();


        int nroR = Random.Range(0, 2);

        GameObject gameSFX = GameObject.Find("GAMESFX");

        if (gameSFX != null)
        {
            audioSource = gameSFX.GetComponent<AudioSource>();
        }
    }
    




    void Update()
    {
        
    }

    public void PlayRemate()
    {
        int nroR = Random.Range(0, rematesSFX.Length);
        viewSFX.RPC("PlayRemateRPC", RpcTarget.All, nroR);
    }

    [PunRPC]
    public void PlayRemateRPC(int nroR)
    {
        audioSource.clip = rematesSFX[nroR];
        audioSource.Play();
    }




    public void PlayPique()
    {
        int nroR2 = Random.Range(0, piquesSFX.Length);
        viewSFX.RPC("PlayPiqueRPC", RpcTarget.All, nroR2);
    }

    [PunRPC]
    public void PlayPiqueRPC(int nroR2)
    {
        audioSource.clip = piquesSFX[nroR2];
        audioSource.Play();
    }




    public void PlayPitido()
    {
        viewSFX.RPC("PlayPitidoRPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayPitidoRPC()
    {
        audioSource.clip = pitidoSFX;
        audioSource.Play();
    }
}
