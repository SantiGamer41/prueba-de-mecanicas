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
        LeanTween.move (texto, new Vector2(4,8), 5.0f);
        //LeanTween.moveX(texto, 8, 5);
        //LeanTween.scaleGUI(texto, new Vector3(200, 400, 1), 2);
    }
}
