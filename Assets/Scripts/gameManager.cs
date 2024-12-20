﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public enum estado
{
    SaqueP1,
    SaqueP2,
    AtaqueP1DefensaP2,
    AtaqueP2DefensaP1,
}
public class gameManager : MonoBehaviourPun
{
    public estado estado;
    [Space(25)]
    [Header("Scripts")]
    DisplayPuntosScript displayPuntosScript;
    public leantweenScript LeantweenScript;
    marcadorScript marcadorScript;
    public photonPlayerSFX sfxScript;
    //public CameraShake cameraShakeScript;
    [Header("UI")]
    private estado estadoActual;
    [Space(25)]
    [Header("Personajes")]
    public GameObject[] personajes; // Array de personajes
    [Space(25)]
    [HideInInspector]
    public GameObject personajeActual;
    [HideInInspector]
    public GameObject personajeBloqueando; // Personaje actualmente seleccionado
    [Header("Objetos")]
    public GameObject ball; //Pelota
    public GameObject casillaIluminadaPrefab;
    public GameObject ballHolderAlto;
    public GameObject ballHolderBajo;
    public GameObject textHolder;
    public GameObject textHolderPopUpLeft;
    public GameObject textHolderPopUpRight;
    private int maxMovementRange;
    public float ballPickupRange = 2.5f;
    private bool enRango = false;
    private bool IsServing;
    [HideInInspector]
    public bool IsBlocking = false;
    [Space(25)]
    private bool isMovingMode = false;
    [Header("Botónes")]
    public GameObject[] botones;
    public Button botonMoverPersonaje; // Referencia al botón en Unity
    public Button botonSacar;
    public Button botonDevolver;
    public Button botonPasar;
    public Button botonArmar;
    public Button botonRematar;
    public Button botonBloquear;
    [Space(25)]
    private bool IsDoingAction = false;
    [Header("Textos")]
    public GameObject textoPointP1;
    public GameObject textoPointP2;
    public GameObject textoSpike;
    [Space(25)]
    private Vector2Int startTile;
    [Header("Animator")]
    public Animator animator;



    public GameObject jugadorPrefab1;
    public GameObject jugadorPrefab2;
    public GameObject jugadorPruebaPrefab;

    PhotonView view;

    public Sprite[] playerSkins;

    public RuntimeAnimatorController[] animators;

    private Animator _animator;
    private PhotonAnimatorView photonAnimatorView;


    private Vector2Int[] posicionesInicialesSaque1 = new Vector2Int[]
  {
        new Vector2Int(-18, -6),
        new Vector2Int(-11, -1),
        new Vector2Int(-5, -6),
        new Vector2Int(-5, -1),
        new Vector2Int(-2, -3),
        new Vector2Int(12, -1),
        new Vector2Int(13, -5),
        new Vector2Int(8, -6),
        new Vector2Int(5, -1),
        new Vector2Int(3, -4)

  };
    private Vector2Int[] posicionesInicialesSaque2 = new Vector2Int[]
  {
        new Vector2Int(-12, -6),
        new Vector2Int(-11, -1),
        new Vector2Int(-5, -6),
        new Vector2Int(-5, -1),
        new Vector2Int(-2, -3),
        new Vector2Int(17, -1),
        new Vector2Int(13, -5),
        new Vector2Int(8, -6),
        new Vector2Int(5, -1),
        new Vector2Int(3, -4)

  };


    // Diccionario para almacenar las casillas por su posición
    private Dictionary<Vector2Int, GameObject> casillasPorPosicion = new Dictionary<Vector2Int, GameObject>();


    void Start()
    {
        estado = estado.SaqueP1;
        estadoActual = estado;
        IsDoingAction = false;

        displayPuntosScript = FindObjectOfType<DisplayPuntosScript>();
        marcadorScript = FindObjectOfType<marcadorScript>();

        view = GetComponent<PhotonView>();

        if (PhotonNetwork.IsConnected)
        {
            AplicarSprites();
        }

        InstanciarCasillas();
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        maxMovementRange = 2; // Ejemplo de rango máximo de movimiento
        InstanciarPersonajesEnPosicionesIniciales();
        //MostrarOpcionesDeArmar();

        


        

        

    }

    private void Update()
    {
    
    }

    public void AplicarSprites()
    {
        Debug.LogError("Se llama aplicar Sprites");
        int playerAvatarIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];

        if (view.IsMine)
        {
            view.RPC("JugadoresIzquierdos", RpcTarget.All, playerAvatarIndex);
        }
        else
        {
            view.RPC("JugadoresDerechos", RpcTarget.All, playerAvatarIndex);
        }
    }

    [PunRPC]
    public void JugadoresIzquierdos(int playerAvatarIndex)
    {
        Debug.LogError("Se llama izquierdos");
        for (int i = 0; i < 5; i++)
        {
            Transform personajeTransform = personajes[i].transform.Find("Personaje");

            if (personajeTransform != null)
            {
                Animator animator = personajeTransform.GetComponent<Animator>();
                if (animator != null)
                {
                    if (playerAvatarIndex < animators.Length)
                    {
                        animator.runtimeAnimatorController = animators[playerAvatarIndex];
                    }

                    SpriteRenderer spriteRenderer = personajeTransform.GetComponent<SpriteRenderer>();

                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = playerSkins[playerAvatarIndex];

                    }

                    animator.SetInteger("AvatarIndex", playerAvatarIndex);

                   

                    foreach (var animatorControllerParameter in animator.parameters)
                    {
                        try
                        {
                            photonAnimatorView.SetParameterSynchronized(animatorControllerParameter.name, PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"Error al sincronizar el parámetro {animatorControllerParameter.name}: {ex.Message}");
                        }
                    }
                    

                }

            }

        }

    }

    [PunRPC]
    public void JugadoresDerechos(int playerAvatarIndex)
    {
        Debug.LogError("Se llama derechos");
        for (int i = 5; i < 10; i++)
        {
            Transform personajeTransform = personajes[i].transform.Find("Personaje");

            if (personajeTransform != null)
            {
                Animator animator = personajeTransform.GetComponent<Animator>();
                if (animator != null)
                {
                    if (playerAvatarIndex < animators.Length)
                    {
                        animator.runtimeAnimatorController = animators[playerAvatarIndex];
                    }

                    SpriteRenderer spriteRenderer = personajeTransform.GetComponent<SpriteRenderer>();

                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = playerSkins[playerAvatarIndex];
                    }

                    animator.SetInteger("AvatarIndex", playerAvatarIndex);
                    

                    foreach (var animatorControllerParameter in animator.parameters)
                    {
                        try
                        {
                            photonAnimatorView.SetParameterSynchronized(animatorControllerParameter.name, PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"Error al sincronizar el parámetro {animatorControllerParameter.name}: {ex.Message}");
                        }
                    }
                    
                }
            }
        }
    }


    GameObject IrARecogerPelota(Vector2Int posicion)
    {
        GameObject personajeMasCercano = null;
        float menorDistancia = float.MaxValue; // Empezamos con la mayor distancia posible

        foreach (GameObject personaje in personajes)
        {
            if (personaje != null && ball != null && ball.transform.parent == null)
            {
                float distancia = CalcularDistancia(personaje, posicion);

                if (distancia <= ballPickupRange && distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    personajeMasCercano = personaje;
                }
            }
        }
        Vector3 posicionV3 = new Vector3(posicion.x, posicion.y, 0);
        if (personajeMasCercano != null)
        {
            StartCoroutine(MovimientoPersonaje(personajeMasCercano.transform.position, posicionV3, personajeMasCercano));
        }
        return personajeMasCercano;
    }

    [PunRPC]
    void RecibirPelotaRPC(int personajeMasCercanoViewID)
    {
        // Obtener el PhotonView del personaje más cercano usando su ViewID
        PhotonView photonViewPersonajeMasCercano = PhotonView.Find(personajeMasCercanoViewID);

        // Verificar si se encontró el PhotonView
        if (photonViewPersonajeMasCercano == null)
        {
            Debug.LogError($"No se encontró el PhotonView con ID {personajeMasCercanoViewID}");
            return; // Salir si no se encuentra el PhotonView
        }

        // Obtener el GameObject asociado al PhotonView
        GameObject personajeMasCercano = photonViewPersonajeMasCercano.gameObject;

        // Llamar a la función que maneja la lógica de recepción de la pelota
        bool enRango = RecibirPelota(personajeMasCercano);

        // Hacer algo con el resultado


    }

    bool RecibirPelota(GameObject personajeMasCercano)
    {
        if (personajeMasCercano != null)
        {
            photonView.RPC("RecogerPelotaRPC", RpcTarget.All, personajeMasCercano.GetPhotonView().ViewID);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InstanciarCasillas()
    {
        for (int x = -17; x <= 19; x++)
        {
            for (int y = -9; y <= 3; y++)
            {
                Vector3 worldPosition = new Vector3(x + 0.5f, y + 0.5f, 0f);
                GameObject casilla = Instantiate(casillaIluminadaPrefab, worldPosition, Quaternion.identity);
                casilla.tag = "Iluminada";
                casilla.SetActive(true); // Ocultar la casilla al inicio

                // Almacenar la casilla en el diccionario
                Vector2Int tilePosition = new Vector2Int(x, y);
                casillasPorPosicion[tilePosition] = casilla;
            }
        }
    }




    [PunRPC]
    public void InstanciarPersonajesEnPosicionesIniciales()
    {
        sfxScript.PlayPitido();

        if (estado == estado.SaqueP1)
        {
            Debug.LogError("Se compara estado");
            for (int i = 0; i < personajes.Length && i < posicionesInicialesSaque1.Length; i++)
            {
                Vector3 worldPosition = new Vector3(posicionesInicialesSaque1[i].x + 0.5f, posicionesInicialesSaque1[i].y, 0f);
                personajes[i].transform.position = worldPosition;
                //Debug.Log("Se instanciaron en" + worldPosition);
            }
        }
        else if (estado == estado.SaqueP2)
        {
            Debug.LogError("Se compara estado");
            for (int i = 0; i < personajes.Length && i < posicionesInicialesSaque2.Length; i++)
            {
                Vector3 worldPosition = new Vector3(posicionesInicialesSaque2[i].x + 0.5f, posicionesInicialesSaque2[i].y, 0f);
                personajes[i].transform.position = worldPosition;
                //Debug.Log("Se instanciaron en" + worldPosition);
            }
        }
    }


    public void SeleccionarPersonaje(int indice)
    {
        if (personajeActual != null && IsDoingAction == false)
        {

            personajeActual.transform.GetChild(1).gameObject.SetActive(false);
            if (indice >= 0 && indice < personajes.Length)
            {

                personajeActual = personajes[indice];
                ObtenerUbicacionDelPersonaje();
                animator = personajeActual.GetComponentInChildren<Animator>();
                personajeActual.transform.GetChild(1).gameObject.SetActive(true);


                // Mostrar botones según el estado
                if (ball.transform.parent == personajeActual.transform)
                {
                    botonSacar.gameObject.SetActive(true);
                    botonDevolver.gameObject.SetActive(true);
                    botonPasar.gameObject.SetActive(true);
                    botonArmar.gameObject.SetActive(true);
                    botonMoverPersonaje.gameObject.SetActive(false);
                }
                else
                {
                    botonDevolver.gameObject.SetActive(false);
                    botonSacar.gameObject.SetActive(false);
                    botonPasar.gameObject.SetActive(false);
                    botonArmar.gameObject.SetActive(false);
                    botonMoverPersonaje.gameObject.SetActive(true);
                }

            }
        }
        else if (personajeActual == null && IsDoingAction == false)
        {

            if (indice >= 0 && indice < personajes.Length)
            {

                personajeActual = personajes[indice];
                ObtenerUbicacionDelPersonaje();
                animator = personajeActual.GetComponentInChildren<Animator>();
                personajeActual.transform.GetChild(1).gameObject.SetActive(true);


                // Mostrar botones según el estado
                if (ball.transform.parent == personajeActual.transform)
                {
                    botonSacar.gameObject.SetActive(true);
                    botonDevolver.gameObject.SetActive(true);
                    botonMoverPersonaje.gameObject.SetActive(false);
                }
                else
                {
                    botonDevolver.gameObject.SetActive(false);
                    botonSacar.gameObject.SetActive(false);
                    botonMoverPersonaje.gameObject.SetActive(true);
                }

            }
        }
    }
    private bool IntentarRecogerPelota(GameObject personaje)
    {
        if (personaje != null && ball != null && ball.transform.parent == null)
        {

            Vector2Int ballPosition = GetGridPosition(ball.transform.position);
            Vector2Int personajePosition = GetGridPosition(personaje.transform.GetChild(2).position);

            float distance = Vector2Int.Distance(personajePosition, ballPosition);

            if (distance <= ballPickupRange)
            {

                animator = personaje.GetComponentInChildren<Animator>();
                animator.SetBool("receive", true);
                ball.transform.SetParent(personaje.transform);
                if (ballPosition.x < 1)
                {
                    ball.transform.localPosition = new Vector3(7, 12, 0);
                }
                else
                {
                    ball.transform.localPosition = new Vector3(-7, 12, 0);
                }

                return true;//animator.SetBool("IsRecieving", false);
            }
            return false;
        }
        return false;
    }

    [PunRPC]
    void RecogerPelotaRPC(int personajeMasCercanoViewID)
    {
        // Obtener el GameObject del personaje más cercano usando su ViewID
        GameObject personajeMasCercano = PhotonView.Find(personajeMasCercanoViewID)?.gameObject;

        // Verificar si el personaje más cercano es nulo
        if (personajeMasCercano == null)
        {
            Debug.LogError("No se pudo encontrar el GameObject con el ViewID: " + personajeMasCercanoViewID);
            return; // Salir de la función si no se encuentra el GameObject
        }

        // Llamar a la función que maneja la lógica de recoger la pelota
        RecogerPelota(personajeMasCercano);
    }


    void RecogerPelota(GameObject personaje)
    {
        animator = personaje.GetComponentInChildren<Animator>();
        animator.SetBool("receive", true);
        ball.transform.SetParent(personaje.transform);

        Vector2Int ballPosition = GetGridPosition(ball.transform.position);

        if (ballPosition.x < 1)
        {
            ball.transform.localPosition = new Vector3(7, 12, 0);
        }
        else
        {
            ball.transform.localPosition = new Vector3(-7, 12, 0);
        }
        /*
        foreach (GameObject p in personajes)
        {
            Animator otherAnimator = p.GetComponentInChildren<Animator>();
            if (p != personaje)
            {
                // Congelar la animación de los otros personajes
                otherAnimator.speed = 0;
            }
        }
        */
        // Cambiar el estado en función de la posición de la pelota
        if (ball.transform.position.x < 1)
        {
            estado = estado.AtaqueP1DefensaP2;
        }
        else
        {
            estado = estado.AtaqueP2DefensaP1;
        }
    }

    public void ActivarBotonMoverPersonaje()
    {
        botonMoverPersonaje.gameObject.SetActive(true);
    }

    public void ActivarBotonSacar()
    {
        botonSacar.gameObject.SetActive(true);
    }
    public void ObtenerUbicacionDelPersonaje()
    {
        if (personajeActual != null)
        {
            PersonajeScript personajeComponent = personajeActual.GetComponent<PersonajeScript>();
            if (personajeComponent != null)
            {
                startTile = personajeComponent.GetGridPosition(personajeActual.transform.position);
            }
        }
    }

    public void OnBotonMoverPersonajeClick()
    {
        isMovingMode = true;
        ObtenerUbicacionDelPersonaje();
        MostrarCasillasAlcanzables();
    }

    public void OnBotonSacarClick()
    {
        isMovingMode = false;
        view.RPC("sacarParentRPC", RpcTarget.All);
        Sacar();
    }

    public void OnBotonDevolverClick()
    {
        isMovingMode = false;
        view.RPC("sacarParentRPC", RpcTarget.All);
        Devolver();
    }

    public void OnBotonPasarClick()
    {
        isMovingMode = false;
        ball.transform.SetParent(null);
        view.RPC("Pasar", RpcTarget.All);
        DescongelarAnimaciones();
    }
    public void OnBotonArmarClick()
    {
        isMovingMode = false;
        Armar();
        DeactivateAllButtons();
    }

    public void OnBotonRematarClick()
    {
        isMovingMode = false;
        view.RPC("sacarParentRPC", RpcTarget.All);
        Rematar();
    }
    public void OnBotonBloquearClick()
    {
        isMovingMode = false;
        view.RPC("Bloquear", RpcTarget.All);
    }
    public void MoverPersonajeA(Vector3 nuevaPosicion)
    {
        if (personajeActual != null && isMovingMode)
        {
            StartCoroutine(MovimientoPersonaje(personajeActual.transform.position, nuevaPosicion, personajeActual));
            DesactivarCasillasIluminadas();
            botonMoverPersonaje.gameObject.SetActive(false);
            isMovingMode = false;
        }
    }
    /*private void MoverPelotaA(Vector3 nuevaPosicion)
    {
        if (ball != null)
        {
        ball.transform.position = nuevaPosicion;
        }
    }
    */

    public void Sacar()
    {
        //Primero mostramos las casillas del lado contrario
        MostrarLadoContrario();

        //Activamos el sistema de selección de casillas
        StartCoroutine(SeleccionDeSaque(ball.transform.position, personajeActual));

    }

    public void Devolver()
    {
        MostrarLadoContrario();
        StartCoroutine(SeleccionDeDevolver(ball.transform.position));
    }

    [PunRPC]
    public void Pasar()
    {
        if (ball.transform.position.x > 1)
        {
            StartCoroutine(SeleccionDePase(ball.transform.position, personajes[9], new Vector3(8, 15, 0), 2));
        }
        else
        {
            StartCoroutine(SeleccionDePase(ball.transform.position, personajes[4], new Vector3(-8, 15, 0), -2));
        }
    }

    public void Armar()
    {
        MostrarOpcionesDeArmar();
        StartCoroutine(SeleccionDeArmado(ball.transform.position));
    }

    public void Rematar()
    {
        MostrarLadoContrario();
        StartCoroutine(SeleccionDeRemate(ball.transform.position, personajeActual));
    }

    [PunRPC]
    public void Bloquear()
    {
        //if (estado == estado.AtaqueP2DefensaP1 && ball.transform.parent == ballHolderAlto)
        StartCoroutine(SeleccionDeBloqueo(personajeActual, personajeActual.transform.position, new Vector3(personajeActual.transform.position.x, personajeActual.transform.position.y + 1.0f, personajeActual.transform.position.z)));
    }


    public void MostrarCasillasAlcanzables()
    {
        DesactivarCasillasIluminadas(); // Asegurarse de ocultar todas las casillas primero
        DeactivateAllButtons();

        List<Vector2Int> reachableTiles = GetReachableTiles(startTile, maxMovementRange);

        foreach (var tile in reachableTiles)
        {
            if (casillasPorPosicion.ContainsKey(tile))
            {
                casillasPorPosicion[tile].SetActive(true); // Mostrar la casilla alcanzable
            }
        }
    }

    public void MostrarLadoContrario()
    {
        //if(personajeActual <= 4)
        //{
        DesactivarCasillasIluminadas();

        int limiteX = 1;

        foreach (var tilePosition in casillasPorPosicion.Keys)
        {
            // Si el personaje está en la mitad izquierda o exactamente en el limiteX
            if (personajeActual.transform.position.x <= limiteX && tilePosition.x > limiteX)
            {
                casillasPorPosicion[tilePosition].SetActive(true);
            }
            // Si el personaje está en la mitad derecha
            else if (personajeActual.transform.position.x > limiteX && tilePosition.x < limiteX)
            {
                casillasPorPosicion[tilePosition].SetActive(true);
            }
            // Debug.Log("Casilla instanciada en" + tilePosition);
        }


        //}

    }

    public void MostrarOpcionesDeArmar()
    {
        DesactivarCasillasIluminadas();
        Vector3 altap1 = new Vector3(personajes[3].transform.position.x + 1, personajes[3].transform.position.y + 4, personajes[3].transform.position.z);
        Vector3 altap2 = new Vector3(personajes[8].transform.position.x - 2, personajes[8].transform.position.y + 4, personajes[8].transform.position.z);
        Vector3 bajap1 = new Vector3(personajes[2].transform.position.x + 1, personajes[2].transform.position.y + 4, personajes[2].transform.position.z);
        Vector3 bajap2 = new Vector3(personajes[7].transform.position.x - 2, personajes[7].transform.position.y + 4, personajes[7].transform.position.z);


        Vector2Int tileAltaP1 = GetGridPosition(altap1);
        Vector2Int tileBajaP1 = GetGridPosition(bajap1);

        Vector2Int tileAltaP2 = GetGridPosition(altap2);
        Vector2Int tileBajaP2 = GetGridPosition(bajap2);
        if (ball.transform.parent == personajes[9].transform)
        {
            //if (personajes[8].transform.position )
            if (casillasPorPosicion.ContainsKey(tileAltaP2))
            {
                casillasPorPosicion[tileAltaP2].SetActive(true);
            }

            if (casillasPorPosicion.ContainsKey(tileBajaP2))
            {
                casillasPorPosicion[tileBajaP2].SetActive(true);
            }
        }
        else if (ball.transform.parent == personajes[4].transform)
        {
            if (casillasPorPosicion.ContainsKey(tileAltaP1))
            {
                casillasPorPosicion[tileAltaP1].SetActive(true);
            }

            if (casillasPorPosicion.ContainsKey(tileBajaP1))
            {
                casillasPorPosicion[tileBajaP1].SetActive(true);
            }
        }
    }




    private IEnumerator SeleccionDeSaque(Vector3 start, GameObject Sacador)
    {
        IsDoingAction = true;
        bool casillaSeleccionada = false;
        float correccion;
        if (Sacador.transform.position.x < 0)
        {
            correccion = -1.2f;
        }
        else
        {
            correccion = 1.2f;
        }
        Vector2Int casillaObjetivo = Vector2Int.zero;

        // Espera hasta que se seleccione una casilla
        while (!casillaSeleccionada)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPosition = GetGridPosition(mouseWorldPosition);

                if (casillasPorPosicion.ContainsKey(gridPosition) && casillasPorPosicion[gridPosition].activeSelf)
                {
                    List<Vector2Int> CasillasPosibles = GetReachableTiles(gridPosition, 3);
                    int randomIndex = Random.Range(0, CasillasPosibles.Count);
                    casillaObjetivo = CasillasPosibles[randomIndex];
                    casillaSeleccionada = true;
                }
            }

            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        if (estado == estado.SaqueP2)
        {

            StartCoroutine(MovimientoPersonaje(personajes[5].transform.position, new Vector3(12.5f, -0.5f, 0), personajes[5]));
        }
        else if (estado == estado.SaqueP1)
        {

            StartCoroutine(MovimientoPersonaje(personajes[0].transform.position, new Vector3(-12.5f, -6.5f, 0), personajes[0]));
        }
        GameObject personajeMasCercano = IrARecogerPelota(casillaObjetivo);
        Debug.LogError("el personaje mas cercano es" + personajeMasCercano);

        sfxScript.PlayRemate();

        // Mover la pelota a la casilla seleccionada
        float duration = 1.5f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x, casillaObjetivo.y + 0.5f, 0);
        if (personajeMasCercano != null)
        {
            Debug.Log("Se aplico la correccion");
            startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
            endPosition = new Vector3(casillaObjetivo.x + correccion, casillaObjetivo.y + 1.8f, 0);
        }

        Debug.Log($"Posición inicial: {startPosition}, Posición final: {endPosition}");

        // Determina la altura máxima de la parábola
        float heightMax = 2.5f;

        DesactivarCasillasIluminadas();

        while (elapsedTime < duration)
        {
            DeactivateAllButtons();

            // Interpolación en el eje X e Y
            float t = elapsedTime / duration;

            // Interpolación en X e Y con una parábola en Z para la altura
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * heightMax;

            // Debug de la posición de la pelota en cada frame
            //Debug.Log($"Tiempo: {elapsedTime}, Posición de la pelota: {currentPos}, t: {t}");

            // Asignar la nueva posición
            ball.transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;
        Debug.Log($"Pelota llegó a la posición final: {endPosition}");
        Vector3 casillaObjetivoV3 = new Vector3(casillaObjetivo.x, casillaObjetivo.y, 0);
        if (personajeMasCercano != null)
        {
            photonView.RPC("RecibirPelotaRPC", RpcTarget.All, personajeMasCercano.GetPhotonView().ViewID);
        }
        else
        {
            Punto();
        }



        if (estadoActual != estado)
        {
            displayPuntosScript.SumarTurnoDisplay();
        }
        enRango = false;
        yield return new WaitForSeconds(1.2f);
        IsDoingAction = false;

        /*
               if (Sacador.transform.position.x > 1)
               {
                   //Logica para cuando hay punto de saque
                   yield return new WaitForSeconds(2);
                   StartCoroutine(MovimientoPersonaje(personajes[5].transform.position, new Vector3(12.5f, -0.5f, 0), personajes[5]));
               }
               else if (Sacador.transform.position.x < 1)
               {
                   yield return new WaitForSeconds(2);
                   StartCoroutine(MovimientoPersonaje(personajes[0].transform.position, new Vector3(-12.5f, -6.5f, 0), personajes[0]));
               }   
         */

    }

    private IEnumerator SeleccionDeDevolver(Vector3 start)
    {
        IsDoingAction = true;
        bool casillaSeleccionada = false;
        float correccion;

        // Determina la corrección según la posición del personaje actual
        if (personajeActual.transform.position.x < 0)
        {
            correccion = -1.2f;
        }
        else
        {
            correccion = 1.2f;
        }

        Vector2Int casillaObjetivo = Vector2Int.zero;

        // Espera hasta que se seleccione una casilla
        while (!casillaSeleccionada)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPosition = GetGridPosition(mouseWorldPosition);

                if (casillasPorPosicion.ContainsKey(gridPosition) && casillasPorPosicion[gridPosition].activeSelf)
                {
                    List<Vector2Int> CasillasPosibles = GetReachableTiles(gridPosition, 3);
                    int randomIndex = Random.Range(0, CasillasPosibles.Count);
                    casillaObjetivo = CasillasPosibles[randomIndex];
                    casillaSeleccionada = true;
                }
            }

            yield return null;
        }

        // Mover la pelota a la casilla seleccionada
        float duration = 1.5f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x + correccion, casillaObjetivo.y + 1.8f, 0);

        // Lógica para el personaje más cercano
        GameObject personajeMasCercano = IrARecogerPelota(casillaObjetivo);
        Debug.Log("El personaje más cercano es: " + personajeMasCercano);

        // Desactivar las casillas iluminadas
        DesactivarCasillasIluminadas();

        // Movimiento de la pelota
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();

            // Interpolación en el eje X e Y
            float t = elapsedTime / duration;

            // Interpolación en X e Y con una parábola en Z para la altura
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * 2.5f; // Altura máxima de la parábola

            // Asignar la nueva posición
            ball.transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;
        Debug.Log($"Pelota llegó a la posición final: {endPosition}");

        // Llama a la RPC para notificar a otros jugadores
        if (personajeMasCercano != null)
        {
            photonView.RPC("RecibirPelotaRPC", RpcTarget.All, personajeMasCercano.GetPhotonView().ViewID);
        }
        else
        {
            Punto();
        }

        // Desactivar botones y animaciones
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("endreceive");
        personajeActual.GetComponentInChildren<Animator>().SetBool("receive", false);

        // Actualiza el estado de puntos
        if (estadoActual != estado)
        {
            displayPuntosScript.SumarTurnoDisplay();
        }

        IsDoingAction = false;
    }


    private IEnumerator SeleccionDePase(Vector3 start, GameObject armador, Vector3 posicionArmadoDePelota, int correccionDePase)
    {
        float duration = 1.5f; // Duración de la animación
        float elapsedTime = 0;

        // Ajusta la posición inicial al centro de la casilla
        Vector3 startPosition = new Vector3(start.x, start.y, start.z);
        Vector3 endPosition = new Vector3(armador.transform.position.x + correccionDePase, armador.transform.position.y + 3, armador.transform.position.z); // end ya está ajustado en MoverPersonajeA

        // Imprime las posiciones ajustadas
        DeactivateAllButtons();
        //personajeActual.GetComponentInChildren<Animator>().SetTrigger("Pass");
        armador.GetComponentInChildren<SpriteRenderer>().flipX = !armador.GetComponentInChildren<SpriteRenderer>().flipX;
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("endreceive");
        personajeActual.GetComponentInChildren<Animator>().SetBool("receive", false);
        float heightMax = 2.5f;

        DesactivarCasillasIluminadas();

        while (elapsedTime < duration)
        {
            DeactivateAllButtons();

            // Interpolación en el eje X e Y
            float t = elapsedTime / duration;

            // Interpolación en X e Y con una parábola en Z para la altura
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * heightMax;

            // Debug de la posición de la pelota en cada frame
            Debug.Log($"Tiempo: {elapsedTime}, Posición de la pelota: {currentPos}, t: {t}");

            // Asignar la nueva posición
            ball.transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ball.transform.parent = armador.transform;
        ball.transform.localPosition = posicionArmadoDePelota;
        /*
        foreach (GameObject p in personajes)
        {
            Animator otherAnimator = p.GetComponentInChildren<Animator>();
            if (p != armador)
            {
                // Congelar la animación de los otros personajes
                otherAnimator.speed = 0;
            }
        }
        //Congelaranimaciones
        */
    }

    private IEnumerator SeleccionDeArmado(Vector3 start)
    {
        Vector3 altap1 = new Vector3(personajes[3].transform.position.x + 1, personajes[3].transform.position.y + 4, personajes[3].transform.position.z);
        Vector3 altap2 = new Vector3(personajes[8].transform.position.x - 2, personajes[8].transform.position.y + 4, personajes[8].transform.position.z);
        Vector3 bajap1 = new Vector3(personajes[2].transform.position.x + 1, personajes[2].transform.position.y + 4, personajes[2].transform.position.z);
        Vector3 bajap2 = new Vector3(personajes[7].transform.position.x - 2, personajes[7].transform.position.y + 4, personajes[7].transform.position.z);
        Vector2Int tileAltaP1 = GetGridPosition(altap1);
        Vector2Int tileBajaP1 = GetGridPosition(bajap1);

        Vector2Int tileAltaP2 = GetGridPosition(altap2);
        Vector2Int tileBajaP2 = GetGridPosition(bajap2);

        view.RPC("sacarParentRPC", RpcTarget.All);

        bool casillaSeleccionada = false;
        Vector2Int casillaObjetivo = Vector2Int.zero;

        // Espera hasta que se seleccione una casilla
        while (!casillaSeleccionada)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPosition = GetGridPosition(mouseWorldPosition);

                if (casillasPorPosicion.ContainsKey(gridPosition) && casillasPorPosicion[gridPosition].activeSelf)
                {
                    casillaObjetivo = gridPosition;
                    casillaSeleccionada = true;
                }
                Debug.Log(casillaObjetivo);
            }

            yield return null;
        }
        IsDoingAction = true;
        DescongelarAnimaciones();

        // Mover la pelota a la casilla seleccionada
        float duration = 1.0f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x, casillaObjetivo.y, 0);
        //Debug.Log("Pepe" + endPosition);
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("Pass");
        float heightMax = 1.5f;

        DesactivarCasillasIluminadas();

        while (elapsedTime < duration)
        {
            DeactivateAllButtons();

            // Interpolación en el eje X e Y
            float t = elapsedTime / duration;

            // Interpolación en X e Y con una parábola en Z para la altura
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * heightMax;

            // Debug de la posición de la pelota en cada frame
            //Debug.Log($"Tiempo: {elapsedTime}, Posición de la pelota: {currentPos}, t: {t}");

            // Asignar la nueva posición
            ball.transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(1);
        photonView.RPC("ActualizarEstadoBola", RpcTarget.All, ball.transform.position);
    }

    [PunRPC]
    public void ActualizarEstadoBola(Vector3 ballPosition)
    {
        Debug.LogError("Se llama RPC actualizar");
        // Cambia la dirección del sprite
        personajeActual.GetComponentInChildren<SpriteRenderer>().flipX = !personajeActual.GetComponentInChildren<SpriteRenderer>().flipX;

        // Cambia el padre de la bola según su posición
        if (ballPosition.y > 2)
        {
            ball.transform.parent = ballHolderAlto.transform;
        }
        else if (ballPosition.y < -2)
        {
            ball.transform.parent = ballHolderBajo.transform;
        }

        // Actualiza el estado de la acción
        IsDoingAction = false;
    }
    [PunRPC]
    public void sacarParentRPC()
    {
        Debug.LogError("Se llama RPC pancho");
        ball.transform.SetParent(null);
    }

    public IEnumerator SeleccionDeRemate(Vector3 start, GameObject rematador)
    {
        IsDoingAction = true;
        bool casillaSeleccionada = false;
        float probabilidadDeBloqueo;
        float correccion;

        Debug.LogError("Se llama la funcion");

        // Determina la corrección según la posición del rematador
        correccion = rematador.transform.position.x < 0 ? -1.2f : 1.2f;

        Vector2Int casillaObjetivo = Vector2Int.zero;

        // Espera hasta que se seleccione una casilla
        while (!casillaSeleccionada)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPosition = GetGridPosition(mouseWorldPosition);

                if (casillasPorPosicion.ContainsKey(gridPosition) && casillasPorPosicion[gridPosition].activeSelf)
                {
                    List<Vector2Int> CasillasPosibles = GetReachableTiles(gridPosition, 2);
                    if (CasillasPosibles.Count > 0) // Asegúrate de que hay casillas posibles
                    {
                        int randomIndex = Random.Range(0, CasillasPosibles.Count);
                        casillaObjetivo = CasillasPosibles[randomIndex];
                        casillaSeleccionada = true;
                    }
                }
            }

            yield return null;
        }

        Vector3 casillaObjetivoV3 = new Vector3(casillaObjetivo.x, casillaObjetivo.y, 0);
        DescongelarAnimaciones();
        probabilidadDeBloqueo = Random.Range(0f, 1f);
        Debug.LogError(probabilidadDeBloqueo);

        // Lógica de bloqueo
        if (IsBlocking && probabilidadDeBloqueo > 0.35f)
        {
            if ((personajeBloqueando.transform.position.y < -4 && rematador.transform.position.y < -4) ||
                (personajeBloqueando.transform.position.y > -4 && rematador.transform.position.y > -4))
            {
                Debug.LogError("Caso de bloqueo");
                StartCoroutine(PelotaBloqueada(personajeActual));
                yield break; // Salir de la función si la pelota está bloqueada
            }
        }

        Debug.LogError("Caso 3 - No bloqueada");
        IsBlocking = false;
        // Desactivar animaciones de bloqueo
        foreach (var personaje in personajes)
        {
            personaje.GetComponentInChildren<Animator>().SetBool("Block", false);
        }

        // Mover la pelota a la casilla seleccionada
        float duration = 1.0f;
        float elapsedTime = 0;
        GameObject personajeMasCercano = IrARecogerPelota(casillaObjetivo);
        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x, casillaObjetivo.y + 0.5f, 0);

        if (personajeMasCercano != null)
        {
            Debug.Log("Se aplicó la corrección");
            endPosition = new Vector3(casillaObjetivo.x + correccion, casillaObjetivo.y + 1.8f, 0);
        }

        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("Spike");
        yield return new WaitForSeconds(0.7f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.12f);
        sfxScript.PlayRemate();
        Time.timeScale = 1.0f;
        //cameraShakeScript.Shake(0.5f, 5f);
        LeantweenScript.AparecerTextoPunto(textoSpike, textHolderPopUpLeft);

        // Movimiento de la pelota
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            // Rotación de la pelota
            Quaternion startRotation = ball.transform.rotation; // rotación inicial
            Quaternion endRotation = Quaternion.Euler(0, 360, 0); // rotación final (ejemplo de 360 grados en Y)
            ball.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            yield return null;
        }

        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;

        // Notificar a otros jugadores sobre la recepción de la pelota
        if (personajeMasCercano != null)
        {
            photonView.RPC("RecibirPelotaRPC", RpcTarget.All, personajeMasCercano.GetPhotonView().ViewID);
        }
        else
        {
            Punto();
        }

        // Actualiza el estado de puntos si es necesario
        if (estadoActual != estado)
        {
            displayPuntosScript.SumarTurnoDisplay();
        }

        // Finaliza la acción
        IsDoingAction = false;
    }

    private IEnumerator SeleccionDeBloqueo(GameObject personaje, Vector3 start, Vector3 end)
    {
        Debug.LogError("Funcion seleccionBloqueo");
        personajeBloqueando = personaje;
        /* float duration = 1.0f; // Duración de la animación
         float elapsedTime = 0;

         // Ajusta la posición inicial al centro de la casilla
         Vector3 startPosition = new Vector3(start.x, start.y, start.z);
         Vector3 endPosition = new Vector3(end.x, end.y, end.z);

         */// Imprime las posiciones ajustadas
        DeactivateAllButtons();
        personaje.GetComponentInChildren<Animator>().SetBool("Block", true);
        /*while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            personaje.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        *///}
        IsBlocking = true;
        yield return null;

    }

    public IEnumerator MovimientoPersonaje(Vector3 start, Vector3 end, GameObject personaje)
    {
        IsDoingAction = true;
        float duration = 1.0f; // Duración de la animación
        float elapsedTime = 0;

        // Ajusta la posición inicial al centro de la casilla
        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(end.x, end.y - 0.5f, end.z); // end ya está ajustado en MoverPersonajeA

        DeactivateAllButtons();
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            personaje.GetComponentInChildren<Animator>().SetBool("IsMoving", true);
            personaje.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        personaje.transform.position = endPosition; // Asegurar que el personaje termine exactamente en la posición final
        personaje.GetComponentInChildren<Animator>().SetBool("IsMoving", false);
        IsDoingAction = false;
    }

    private void DesactivarCasillasIluminadas()
    {
        foreach (var casilla in casillasPorPosicion.Values)
        {
            casilla.SetActive(false); // Ocultar la casilla
        }
    }

    bool IsTileAccessible(Vector2Int tile)
    {
        List<Vector2Int> nonAccessibleTiles = new List<Vector2Int>
        {
            // Limite de red
            new Vector2Int(1, -5), new Vector2Int(1, -7), new Vector2Int(1, -6),
            new Vector2Int(1, -5), new Vector2Int(1, -4), new Vector2Int(0, -3),
            new Vector2Int(0, -2), new Vector2Int(0, -1), new Vector2Int(-1, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(-1, 2),
            new Vector2Int(-2, 2),
            // Limite superior
            new Vector2Int(-15, 1), new Vector2Int(-14, 1), new Vector2Int(-13, 1),
            new Vector2Int(-12, 1), new Vector2Int(-11, 1), new Vector2Int(-10, 1),
            new Vector2Int(-9, 1), new Vector2Int(-8, 1), new Vector2Int(-7, 1),
            new Vector2Int(-6, 1), new Vector2Int(-5, 1), new Vector2Int(-4, 1),
            new Vector2Int(-3, 1), new Vector2Int(-2, 1), new Vector2Int(-1, 1),
            new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1),
            new Vector2Int(3, 1), new Vector2Int(4, 1), new Vector2Int(5, 1),
            new Vector2Int(6, 1), new Vector2Int(7, 1), new Vector2Int(8, 1),
            new Vector2Int(9, 1), new Vector2Int(10, 1), new Vector2Int(11, 1),
            new Vector2Int(12, 1), new Vector2Int(13, 1),
            // Limite izquierdo
            new Vector2Int(-17, -7), new Vector2Int(-17, -6), new Vector2Int(-17, -5),
            new Vector2Int(-16, -4),new Vector2Int(-16, -3), new Vector2Int(-15, -2),
            new Vector2Int(-15, -1),new Vector2Int(-15, 0), new Vector2Int(-14, 1),
            // Limite Derecho
            new Vector2Int(19, -7), new Vector2Int(19, -6), new Vector2Int(18, -5),
            new Vector2Int(17, -4),new Vector2Int(16, -3),new Vector2Int(16, -2),
            new Vector2Int(15, -1),new Vector2Int(14, 0),new Vector2Int(13, 1),
        };

        if (nonAccessibleTiles.Contains(tile))
        {
            return false; // No accesible
        }

        Vector2Int topLeftInferior = new Vector2Int(-17, -8);
        Vector2Int bottomRightInferior = new Vector2Int(19, -9);
        Vector2Int topLeftSuperior = new Vector2Int(-13, 4);
        Vector2Int bottomRightSuperior = new Vector2Int(-13, 5);

        if (tile.x >= topLeftInferior.x && tile.x <= bottomRightInferior.x && tile.y >= bottomRightInferior.y && tile.y <= topLeftInferior.y)
        {
            return false; // No accesible
        }

        return true; // Accesible
    }

    List<Vector2Int> GetReachableTiles(Vector2Int startTile, int maxMovementRange)
    {
        List<Vector2Int> reachableTiles = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startTile);
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        distances[startTile] = 0;

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // Arriba
            new Vector2Int(0, -1),  // Abajo
            new Vector2Int(-1, 0),  // Izquierda
            new Vector2Int(1, 0)    // Derecha
        };

        while (queue.Count > 0)
        {
            Vector2Int currentTile = queue.Dequeue();
            int currentDistance = distances[currentTile];

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = currentTile + dir;

                if (!distances.ContainsKey(neighbor) && IsTileAccessible(neighbor))
                {
                    int newDistance = currentDistance + 1;

                    if (newDistance <= maxMovementRange)
                    {
                        distances[neighbor] = newDistance;
                        queue.Enqueue(neighbor);
                        reachableTiles.Add(neighbor);
                    }
                }
            }
        }

        return reachableTiles;
    }
    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int xpos = Mathf.RoundToInt(worldPosition.x);
        int ypos = Mathf.RoundToInt(worldPosition.y);
        return new Vector2Int(xpos, ypos);
    }
    public void DeactivateAllButtons()
    {
        for (int i = 0; i <= botones.Length - 1; i++)
        {
            //Debug.Log(botones[i].name);
            botones[i].SetActive(false);
        }
    }
    float CalcularDistancia(GameObject personaje, Vector2Int casillaObjetivo)
    {
        Vector2Int ballPosition = casillaObjetivo;
        Vector2Int personajePosition = GetGridPosition(personaje.transform.GetChild(2).position);

        return Vector2Int.Distance(personajePosition, ballPosition);
    }

    public void NoRematar()
    {
        if (ball.transform.parent == ballHolderBajo.transform || ball.transform.parent == ballHolderAlto.transform)
        {
            botonRematar.gameObject.SetActive(false);
        }
    }

    public void DescongelarAnimaciones()
    {
        foreach (GameObject p in personajes)
        {
            Animator animator = p.GetComponentInChildren<Animator>();

            // Restaurar la velocidad de la animación
            animator.speed = 1;

        }
    }

    public IEnumerator PelotaBloqueada(GameObject rematador)
    {
        Debug.Log("PELOTA BLOQUEADA");
        DescongelarAnimaciones();
        float duration = 0.5f;
        float halfDuration = duration / 2f;
        float elapsedTime = 0;
        if (ball.transform.position.x < 0)
        {
            if (ball.transform.position.y < -1)
            {
                personajeBloqueando = personajes[7];
            }
            else
            {
                personajeBloqueando = personajes[8];
            }
        }
        else
        {
            if (ball.transform.position.y < -1)
            {
                personajeBloqueando = personajes[2];
            }
            else
            {
                personajeBloqueando = personajes[3];
            }
        }

        Vector3 startPosition = ball.transform.position;
        Debug.Log(startPosition);
        Vector3 midPosition = new Vector3(
        personajeBloqueando.transform.position.x,
        startPosition.y + 2.0f,
        startPosition.z);
        Vector3 endPosition = startPosition;
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        rematador.GetComponentInChildren<Animator>().SetTrigger("Spike");
        yield return new WaitForSeconds(0.7f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.12f);
        Time.timeScale = 0.4f;
        //LeantweenScript.AparecerTextoPunto(textoBlock, textHolderPopUpRight);
        while (elapsedTime < halfDuration)
        {
            ball.transform.position = Vector3.Lerp(startPosition, midPosition, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ball.transform.position = midPosition;

        elapsedTime = 0;
        while (elapsedTime < halfDuration)
        {
            ball.transform.position = Vector3.Lerp(midPosition, endPosition, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        personajes[2].GetComponentInChildren<Animator>().SetBool("Block", false);
        personajes[3].GetComponentInChildren<Animator>().SetBool("Block", false);
        personajes[7].GetComponentInChildren<Animator>().SetBool("Block", false);
        personajes[8].GetComponentInChildren<Animator>().SetBool("Block", false);

        ball.transform.position = endPosition;

        Punto();

        Debug.Log("Pelota llegó al punto final.");
        Time.timeScale = 1f;
        IsBlocking = false;
    }

    public void Punto()
    {
        if (ball.transform.position.x > 1)
        {
            if (ball.transform.position.x < 18 && ball.transform.position.y < 1 && ball.transform.position.y > -8)
            {
                marcadorScript.SumarPuntosLeft();
            }
            else
            {
                marcadorScript.SumarMarcadorRight();
            }
        }
        else if (ball.transform.position.x < 1)
        {
            if (ball.transform.position.x > -16 && ball.transform.position.y < 1 && ball.transform.position.y > -8)
            {
                marcadorScript.SumarMarcadorRight();
            }
            else
            {
                marcadorScript.SumarPuntosLeft();
            }
        }
        photonView.RPC("PuntoRPC", RpcTarget.All);
    }


    [PunRPC]
    public void PuntoRPC()
    {
        if (enRango == false)
        {
            Debug.LogError("Else rpc");
            IsDoingAction = false;

            // Verificar si la bola está inicializada
            if (ball == null)
            {
                Debug.LogError("La bola no está inicializada.");
                return; // Salir si la bola no está inicializada
            }

            Debug.Log($"Estado actual: {estado}, Posición de la bola: {ball.transform.position}");

            // Lógica para el saque basado en la posición de la bola
            if (ball.transform.position.x > 1)
            {
                if (ball.transform.position.x < 18 && ball.transform.position.y < 1 && ball.transform.position.y > -8)
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP1, textHolder);
                    Debug.Log("Se mostró el punto");
                    estado = estado.SaqueP1;
                    ball.transform.parent = personajes[0].transform;
                    ball.transform.localPosition = new Vector3(7, 12, 0);
                    photonView.RPC("InstanciarPersonajesEnPosicionesIniciales", RpcTarget.All);
                    
                }
                else
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP2, textHolder);
                    estado = estado.SaqueP2;
                    ball.transform.parent = personajes[5].transform;
                    ball.transform.localPosition = new Vector3(-7, 12, 0);
                    photonView.RPC("InstanciarPersonajesEnPosicionesIniciales", RpcTarget.All);
                }
            }
            else if (ball.transform.position.x < 1)
            {
                if (ball.transform.position.x > -16 && ball.transform.position.y < 1 && ball.transform.position.y > -8)
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP2, textHolder);
                    estado = estado.SaqueP2;
                    ball.transform.parent = personajes[5].transform;
                    ball.transform.localPosition = new Vector3(-7, 12, 0);
                    photonView.RPC("InstanciarPersonajesEnPosicionesIniciales", RpcTarget.All);
                }
                else
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP1, textHolder);
                    estado = estado.SaqueP1;
                    ball.transform.parent = personajes[0].transform;
                    ball.transform.localPosition = new Vector3(7, 12, 0);
                    photonView.RPC("InstanciarPersonajesEnPosicionesIniciales", RpcTarget.All);
                }
            }
        }
    }

}