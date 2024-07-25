using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public gameManager GameManager; // Referencia al GameManager
     public int personajeIndice;

    private void OnMouseDown()
    {
        // Llamar al método para activar el botón en el GameManager
        GameManager.SeleccionarPersonaje(personajeIndice);
        GameManager.ActivarBotonMoverPersonaje();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
