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
        if (GameManager.estado == estado.SaqueP1) //Saque P1
        {
        switch (personajeIndice)
        {
                case 9:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();

                    break;
                case 8:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 7:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 6:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 5:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    break;
                //Equipo 1
                case 4:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                break;
            case 3:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                break;
            case 2:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                break;
            case 1:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                break;
            case 0:
                GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonDevolver.gameObject.SetActive(false);
                    GameManager.ActivarBotonSacar();
                break;
        
       
        }
        }
        else if (GameManager.estado == estado.AtaqueP2DefensaP1) //Ataque P2 Defensa P1
        {
            switch (personajeIndice)
            {
            //Equipo 2
            case 9:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
            case 8:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
            case 7:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
            case 6:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
            case 5:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
            //Equipo 1
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
                GameManager.StartCoroutine(GameManager.MovimientoPersonaje(GameManager.personajeActual.transform.position, new Vector3(-12, -6, 0)));
                break;
        
            }

        }
        else if (GameManager.estado == estado.AtaqueP1DefensaP2)
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
                //Equipo 1
                case 4:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
                case 3:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
                case 2:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
                case 1:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    break;
                case 0:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.StartCoroutine(GameManager.MovimientoPersonaje(GameManager.personajeActual.transform.position, new Vector3(-12, -6, 0)));
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
