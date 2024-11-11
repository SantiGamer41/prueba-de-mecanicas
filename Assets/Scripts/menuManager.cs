using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public Animator anim;
    public GameObject panel;
    public GameObject creditos;

    void Start()
    {
        audioManager.Instance.PlayMusic(menuMusic);
    }
    void Update()
    {

    }
    public void OnCreditsClick()
    {
        creditos.SetActive(true);
    }
    public void OnVolverClick()
    {
        creditos.SetActive(false);
        panel.SetActive(true);
    }
    public void OnClick()
    {
        SceneManager.LoadScene("InicioMultijugador");

        //StartCoroutine(LoadScene("InicioMultijugador"));
    }

    /*
    public IEnumerator LoadScene(string scene)
    {
        anim.SetTrigger("endscene");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(scene);
    }
    */
        
}
