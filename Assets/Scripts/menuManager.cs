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

   

    // Start is called before the first frame update
    void Start()
    {
        audioManager.Instance.PlayMusic(menuMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCreditsClick()
    {
        StartCoroutine(LoadPanel(creditos));
    }
    public void OnVolverClick()
    {
        Debug.Log("volvido");
        StartCoroutine(LoadPanel(panel));
    }
    public void OnClick()
    {
        StartCoroutine(LoadScene("InicioMultijugador"));
    }
   
    public IEnumerator LoadScene(string scene)
    {
        anim.SetTrigger("endscene");
        yield return new WaitForSeconds(1.1f);
         SceneManager.LoadScene(scene);
    }

    private IEnumerator LoadPanel(GameObject panel)
    {
        anim.SetTrigger("endscene");
        yield return new WaitForSeconds(1.1f);

        // Desactivar todos los paneles primero
        if (panel != null) panel.SetActive(false);
        if (creditos != null) creditos.SetActive(false);

        // Activar el panel que se desea mostrar
        panel.SetActive(true);
    }
}
