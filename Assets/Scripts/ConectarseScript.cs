using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class ConectarseScript : MonoBehaviourPunCallbacks
{
    public InputField usernameInput;
    public Text buttonText;
    public Animator anim;
    public GameObject ConectandoseButton;

    private void Start()
    {
        anim.SetTrigger("endscene");
    }

    public void ClickConectarse()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;
            ConectandoseButton.SetActive(true);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        StartCoroutine(LoadScene("LobbyMultijugador"));
    }
    public IEnumerator LoadScene(string scene)
    {
        anim.SetTrigger("endscene");
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(scene);
    }
    public void OnClickBtnVolver()
    {
        SceneManager.LoadScene("Menu");
    }
}
