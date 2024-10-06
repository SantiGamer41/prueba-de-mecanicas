using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMusic : MonoBehaviour
{
    public AudioClip juegoMusic;

    // Start is called before the first frame update
    void Start()
    {
        audioManager.Instance.PlayMusic(juegoMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
