using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AparecerTextoPuntoJ1(GameObject texto)
    {
        LeanTween.moveY(texto, 4, 2);
        LeanTween.scale(texto, new Vector3(200, 400, 1), 2);
    }
}
