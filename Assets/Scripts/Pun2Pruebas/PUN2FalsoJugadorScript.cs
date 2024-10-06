using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PUN2FalsoJugadorScript : MonoBehaviour
{
    PUN2ContadorScript contadorScript;
    PhotonView View;
    public float speed;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        View = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        contadorScript = FindObjectOfType<PUN2ContadorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (View.IsMine)
        {
            Vector2 moveImput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveAmount = moveImput.normalized * speed * Time.deltaTime;
            transform.position += (Vector3)moveAmount;

            if(moveImput == Vector2.zero)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }
        }
    }
}
