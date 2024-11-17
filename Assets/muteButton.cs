using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class muteButton : MonoBehaviour
{
    public Button buttonMute;
    public Sprite muteOn;
    public Sprite muteOff;
    private static bool soundIsOn = true;

    public AudioSource musicSource; // El componente AudioSource que quieres usar
     audioManager audioManager; // Clase que maneja el audio

    // Start is called before the first frame update
    void Start()
    {
        // Busca el objeto llamado "AUDIO MANAGER" en la jerarquía
        GameObject audioManagerObject = GameObject.Find("AUDIO MANAGER");

        if (audioManagerObject != null)
        {
            // Obtén el componente AudioManager
            audioManager = audioManagerObject.GetComponent<audioManager>();

            if (audioManager != null)
            {
                // Asigna el AudioSource del AudioManager
                musicSource = audioManager.musicSource;
            }
        }

        UpdateMuteButton();
    }
            // Update is called once per frame
    void Update()
    {       
        
    }
    public void OnMuteClick()
    {
        if (soundIsOn)
        {
            buttonMute.image.sprite = muteOn;
            soundIsOn = false;
            musicSource.mute = true;
        }
        else
        {
            buttonMute.image.sprite = muteOff;
            soundIsOn = true;
            musicSource.mute = false;
        }
    }

    public void UpdateMuteButton()
    {
        if (!soundIsOn)
        {
            buttonMute.image.sprite = muteOn;
        }
        else
        {
            buttonMute.image.sprite = muteOff;
        }
    }
}
