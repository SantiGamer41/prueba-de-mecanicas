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
        //If (situacion de remate)
        if (GameManager.estado == estado.SaqueP1)
        {
        switch (personajeIndice)
        {
                case 9:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 8:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 7:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 6:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 5:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 4:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 3:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 2:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 1:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 0:
                GameManager.botonMoverPersonaje1.gameObject.SetActive(false);
                GameManager.ActivarBotonSacar();
                break;
        
       
        }
        }
        else if (GameManager.estado == estado.AtaqueP1DefensaP2)
        {
            switch (personajeIndice)
            {
            case 4:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 3:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 2:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 1:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
            case 0:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                break;
        
            }

        }

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
