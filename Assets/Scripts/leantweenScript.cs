using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class leantweenScript : MonoBehaviour
{
    public Vector2 holderPosition;
    gameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
       gameManager = FindObjectOfType<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        holderPosition = gameManager.textHolder.GetComponent<RectTransform>().position;
    }

    public void AparecerTextoPunto(GameObject texto,  GameObject textHolder)
    {

        LeanTween.move (texto, textHolder.GetComponent<RectTransform>().position, 0.5f);
        LeanTween.scale(texto, new Vector2(1.20f, 1.20f), 0.5f);
        
       
        LeanTween.scale(texto, new Vector2(0f, 0f), 1.0f).setDelay(2);
        LeanTween.move(texto, new Vector2(textHolder.GetComponent<RectTransform>().position.x, textHolder.GetComponent<RectTransform>().position.y - 100), 1.0f).setDelay(2);
        //LeanTween.moveX(texto, 8, 5);
        //LeanTween.scaleGUI(texto, new Vector3(200, 400, 1), 2);
    }
}
