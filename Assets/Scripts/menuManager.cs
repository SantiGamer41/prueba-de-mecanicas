using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private bool isMuted = false; // Variable para controlar el estado del sonido
    public Button muteButton; // Asigna aquí el botón desde el Inspector

    void Start()
    {
        // Agregamos el listener al botón para que ejecute el método ToggleMute al ser clickeado
        muteButton.onClick.AddListener(ToggleMute);
    }

    // Método para activar/desactivar el sonido
    public void ToggleMute()
    {
        if (isMuted)
        {
            // Si está silenciado, lo activamos
            audioManager.Instance.musicSource.mute = false;
            isMuted = false;
        }
        else
        {
            // Si no está silenciado, lo desactivamos
            audioManager.Instance.musicSource.mute = true;
            isMuted = true;
        }
    }
}
