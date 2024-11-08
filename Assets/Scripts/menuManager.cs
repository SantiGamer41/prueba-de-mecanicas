using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private bool isMuted = false; // Variable para controlar el estado del sonido
    public Button muteButton; // Asigna aqu� el bot�n desde el Inspector

    void Start()
    {
        // Agregamos el listener al bot�n para que ejecute el m�todo ToggleMute al ser clickeado
        muteButton.onClick.AddListener(ToggleMute);
    }

    // M�todo para activar/desactivar el sonido
    public void ToggleMute()
    {
        if (isMuted)
        {
            // Si est� silenciado, lo activamos
            audioManager.Instance.musicSource.mute = false;
            isMuted = false;
        }
        else
        {
            // Si no est� silenciado, lo desactivamos
            audioManager.Instance.musicSource.mute = true;
            isMuted = true;
        }
    }
}
