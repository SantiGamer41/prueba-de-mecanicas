using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{
    public static audioManager Instance;
    public AudioSource muscicaMenu;
    public AudioSource muscicaLobby;
    public AudioSource muscicaJuego;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void PlaySoundMenu()
    {
        muscicaMenu.Play();
        muscicaLobby.Stop();
        muscicaJuego.Stop();
    }

    public void PlaySoundLobby()
    {
        muscicaLobby.Play();
        muscicaMenu.Stop();
        muscicaJuego.Stop();
    }

    public void PlaySoundJuego()
    {
        muscicaJuego.Play();
        muscicaMenu.Stop();
        muscicaLobby.Stop();
    }
    public void StopMusic()
    {
        muscicaJuego.Stop();
        muscicaMenu.Stop();
        muscicaLobby.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
