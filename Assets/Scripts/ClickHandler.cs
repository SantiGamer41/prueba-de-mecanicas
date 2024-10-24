using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public int personajeIndice;
    private gameManager GameManager;

    void Start()
    {
        // Intentar encontrar el objeto GameManager por nombre
        GameObject gameManagerObject = GameObject.Find("GAME MANAGER");

        if (gameManagerObject != null)
        {
            // Obtener el componente GameManager
            GameManager = gameManagerObject.GetComponent<gameManager>();

            if (GameManager != null)
            {
                Debug.Log("GameManager encontrado y componente asignado correctamente");
            }
            else
            {
                Debug.LogError("No se pudo encontrar el componente GameManager en el objeto");
            }
        }
        else
        {
            Debug.LogError("No se pudo encontrar el objeto GameManager en la jerarquía");
        }
    }

    void Update()
    {

    }
    private void OnMouseDown()
    {
        Debug.LogError("Me han clickeado");


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
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 8:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 7:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 6:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    Debug.LogErrorFormat("Caso 6");
                    break;
                case 5:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    Debug.LogErrorFormat("Caso 5");
                    break;
                //Equipo 1
                case 4:
                GameManager.DeactivateAllButtons();
                    break;
                case 3:
                GameManager.DeactivateAllButtons();
                    break;
                case 2:
                GameManager.DeactivateAllButtons();
                    Debug.LogErrorFormat("Caso 2");
                    break;
                case 1:
                GameManager.DeactivateAllButtons();
                    Debug.LogErrorFormat("Caso 1");
                    break;
                case 0:
                GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.ActivarBotonSacar();
                    break;
            }
        }
        else if (GameManager.estado == estado.SaqueP2) //Saque P2
        {
            switch (personajeIndice)
            {
                case 4:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 3:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 2:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 1:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                case 0:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                    break;
                //Equipo 2
                case 9:
                GameManager.DeactivateAllButtons();
                    break;
                case 8:
                GameManager.DeactivateAllButtons();
                    break;
                case 7:
                GameManager.DeactivateAllButtons();
                    break;
                case 6:
                GameManager.DeactivateAllButtons();
                    break;
                case 5:
                GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
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
                    GameManager.botonPasar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.personajes[9].transform)
                    {
                    GameManager.botonArmar.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonArmar.gameObject.SetActive(false);
                    }
                    GameManager.NoRematar();
                    break;
            case 8:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.ballHolderAlto.transform)
                    {
                    GameManager.botonRematar.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonRematar.gameObject.SetActive(false);
                    }
                    break;
            case 7:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.ballHolderBajo.transform)
                    {
                    GameManager.botonRematar.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonRematar.gameObject.SetActive(false);
                    }
                    break;
            case 6:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;
            case 5:
                GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;
            //Equipo 1
            case 4:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                    break;
            case 3:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                 if (GameManager.ball.transform.parent == GameManager.ballHolderAlto.transform)
                    {
                    GameManager.botonBloquear.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    }
                    break;
                    
            case 2:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                 if (GameManager.ball.transform.parent == GameManager.ballHolderBajo.transform)
                    {
                    GameManager.botonBloquear.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    }                
                    break;
            case 1:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                    break;
            case 0:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
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
                    GameManager.NoRematar();
                    break;
                case 8:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    break;
                case 7:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    break;
                case 6:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    break;
                case 5:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.personajes[5])
                    {
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    }
                    else
                    {
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    GameManager.NoRematar();
                    break;
                //Equipo 1
                case 4:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonPasar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.personajes[4].transform)
                    {
                    GameManager.botonArmar.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonArmar.gameObject.SetActive(false);
                    }
                    GameManager.NoRematar();
                    break;
                case 3:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.ballHolderAlto.transform)
                    {
                    GameManager.botonRematar.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonRematar.gameObject.SetActive(false);
                    }
                    break;
                case 2:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    if (GameManager.ball.transform.parent == GameManager.ballHolderBajo.transform)
                    {
                    GameManager.botonRematar.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonRematar.gameObject.SetActive(false);
                    }
                    break;
                case 1:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;
                case 0:
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;

            }

        }


    } 
}
