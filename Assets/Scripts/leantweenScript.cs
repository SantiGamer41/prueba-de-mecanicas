using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class leantweenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AparecerTextoPunto(GameObject texto)
    {

        LeanTween.move (texto, new Vector2(220,167), 1.0f);
        LeanTween.scale(texto, new Vector2(2.20f, 2.20f), 1.0f).setDelay(0);
        
       
        LeanTween.scale(texto, new Vector2(0f, 0f), 1.0f).setDelay(2);
        LeanTween.move(texto, new Vector2(220, 10), 1.0f);
        //LeanTween.moveX(texto, 8, 5);
        //LeanTween.scaleGUI(texto, new Vector3(200, 400, 1), 2);
    }
}
