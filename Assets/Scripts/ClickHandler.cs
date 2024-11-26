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
                    break;
                case 5:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonDevolver.gameObject.SetActive(false);
                GameManager.botonPasar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
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
                    break;
                case 1:
                GameManager.DeactivateAllButtons();
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
                    GameManager.botonBloquear.gameObject.SetActive(false);
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
                GameManager.botonBloquear.gameObject.SetActive(false);
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
                GameManager.botonBloquear.gameObject.SetActive(false);
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
                GameManager.botonBloquear.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;
            case 5:
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.botonBloquear.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;
            //Equipo 1
            case 4:
            GameManager.DeactivateAllButtons();
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                    break;
            case 3:
            GameManager.DeactivateAllButtons();
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                 if (GameManager.IsBlocking == false)
                    {
                    GameManager.botonBloquear.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    }
                    break;
                    
            case 2:
            GameManager.DeactivateAllButtons();
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                 if (GameManager.IsBlocking == false)
                    {
                    GameManager.botonBloquear.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    }     
                    break;
            case 1:
            GameManager.DeactivateAllButtons();
                GameManager.botonSacar.gameObject.SetActive(false);
                GameManager.ActivarBotonMoverPersonaje();
                GameManager.NoRematar();
                    break;
            case 0:
            GameManager.DeactivateAllButtons();
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
                GameManager.DeactivateAllButtons();
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    break;
                case 8:
                GameManager.DeactivateAllButtons();
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    if (GameManager.IsBlocking == false)
                    {
                    GameManager.botonBloquear.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    }
                    break;
                case 7:
                GameManager.DeactivateAllButtons();
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    if (GameManager.IsBlocking == false)
                    {
                    GameManager.botonBloquear.gameObject.SetActive(true);
                    }
                    else
                    {
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    }
                    break;
                case 6:
                GameManager.DeactivateAllButtons();
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.ActivarBotonMoverPersonaje();
                    GameManager.NoRematar();
                    break;
                case 5:
                GameManager.DeactivateAllButtons();
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
                    GameManager.botonBloquear.gameObject.SetActive(false);
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
                    GameManager.botonBloquear.gameObject.SetActive(false);
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
                    GameManager.botonBloquear.gameObject.SetActive(false);
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
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
                    break;
                case 0:
                    GameManager.botonBloquear.gameObject.SetActive(false);
                    GameManager.botonSacar.gameObject.SetActive(false);
                    GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                    GameManager.botonArmar.gameObject.SetActive(false);
                    GameManager.NoRematar();
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
