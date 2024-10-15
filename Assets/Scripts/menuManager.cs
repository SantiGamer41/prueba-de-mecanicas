using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public Animator anim;
   

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
        StartCoroutine(LoadScene("InicioMultijugador"));
    }
   
    public IEnumerator LoadScene(string scene)
    {
        anim.SetTrigger("endscene");
        yield return new WaitForSeconds(1.1f);
         SceneManager.LoadScene(scene);
    }
}
