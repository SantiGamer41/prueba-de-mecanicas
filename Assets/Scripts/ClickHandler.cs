using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ClickHandler : MonoBehaviour
{
    public gameManager GameManager; // Referencia al GameManager
    public int personajeIndice;

    PhotonView viewCH;

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
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;

                case 8:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 7:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 6:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 5:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                //Equipo 1


                case 4:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();

                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;


                case 3:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;

                case 2:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 1:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 0:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.botonArmar.gameObject.SetActive(false);
                        GameManager.ActivarBotonSacar();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }

                    break;

            }
        }
        else if (GameManager.estado == estado.SaqueP2) //Saque P2
        {
            switch (personajeIndice)
            {
                case 4:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 3:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 2:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 1:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 0:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                //Equipo 2
                case 9:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 8:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 7:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 6:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 5:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                        GameManager.botonDevolver.gameObject.SetActive(false);
                        GameManager.botonPasar.gameObject.SetActive(false);
                        GameManager.botonArmar.gameObject.SetActive(false);
                        GameManager.ActivarBotonSacar();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
            }
        }
        else if (GameManager.estado == estado.AtaqueP2DefensaP1) //Ataque P2 Defensa P1
        {
            switch (personajeIndice)
            {
                //Equipo 2
                case 9:
                    if (!PhotonNetwork.IsMasterClient)
                    {
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

                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 8:
                    if (!PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 7:
                    if (!PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 6:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonBloquear.gameObject.SetActive(false);
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                        GameManager.botonArmar.gameObject.SetActive(false);
                        GameManager.NoRematar();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 5:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonBloquear.gameObject.SetActive(false);
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                        GameManager.botonArmar.gameObject.SetActive(false);
                        GameManager.NoRematar();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                //Equipo 1
                case 4:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                        GameManager.NoRematar();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 3:
                    if (PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;

                case 2:
                    if (PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 1:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                        GameManager.NoRematar();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 0:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                        GameManager.NoRematar();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;


            }

        }
        else if (GameManager.estado == estado.AtaqueP1DefensaP2)
        {
            switch (personajeIndice)
            {
                case 9:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                        GameManager.NoRematar();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 8:
                    if (!PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 7:
                    if (!PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 6:
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.ActivarBotonMoverPersonaje();
                        GameManager.NoRematar();
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 5:
                    if (!PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                //Equipo 1
                case 4:
                    if (PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 3:
                    if (PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 2:
                    if (PhotonNetwork.IsMasterClient)
                    {
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
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 1:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonBloquear.gameObject.SetActive(false);
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                        GameManager.botonArmar.gameObject.SetActive(false);
                        GameManager.NoRematar();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;
                case 0:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameManager.botonBloquear.gameObject.SetActive(false);
                        GameManager.botonSacar.gameObject.SetActive(false);
                        GameManager.botonMoverPersonaje.gameObject.SetActive(false);
                        GameManager.botonArmar.gameObject.SetActive(false);
                        GameManager.NoRematar();
                    }
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        GameManager.DeactivateAllButtons();
                    }
                    break;

            }

        }


    }
    // Start is called before the first frame update
    void Start()
    {
        viewCH = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
