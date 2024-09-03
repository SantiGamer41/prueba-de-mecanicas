using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyAudio : MonoBehaviour
{
    public AudioClip lobbyMusic;
    // Start is called before the first frame update
    void Start()
    {
        audioManager.Instance.PlayMusic(lobbyMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
