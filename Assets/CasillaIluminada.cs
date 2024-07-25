using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasillaIluminada : MonoBehaviour
{
     private gameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<gameManager>();
    }
     private void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.MoverPersonajeA(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
