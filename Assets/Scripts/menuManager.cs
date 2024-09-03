using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public AudioClip menuMusic;

    // Start is called before the first frame update
    void Start()
    {
        audioManager.Instance.PlayMusic(menuMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        SceneManager.LoadScene("InicioMultijugador");
    }
}
