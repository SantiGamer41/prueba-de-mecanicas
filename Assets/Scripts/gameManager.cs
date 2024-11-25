﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using System;

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
    [Header("UI")]
    private estado estadoActual;
    [Space(25)]
    [Header("Personajes")]
    public GameObject[] personajes;
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
    private bool IsBlocking = false;
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

    public Sprite[] playerSkins;

    public RuntimeAnimatorController[] animators;


    PhotonView view;

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
        InstanciarCasillas();
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        maxMovementRange = 2; // Ejemplo de rango máximo de movimiento
        InstanciarPersonajesEnPosicionesIniciales();
        //MostrarOpcionesDeArmar();

        displayPuntosScript = FindObjectOfType<DisplayPuntosScript>();
        view = GetComponent<PhotonView>();

        

        if (PhotonNetwork.IsConnected)
        {
            SpawnJugadores();
            AplicarSprites();
        }


        
    }

    private void Update()
    {
        
    }


    private List<GameObject> jugadoresLocales = new List<GameObject>(); // Lista local de jugadores instanciados

    public void AplicarSprites()
    {
        int playerAvatarIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];

        // Verificar que el índice esté dentro del rango
        if (playerAvatarIndex < 0 || playerAvatarIndex >= playerSkins.Length)
        {
            Debug.LogError("Índice de avatar fuera de rango.");
            return;
        }

        // Obtener el sprite correspondiente al jugador local
        Sprite playerSprite = playerSkins[playerAvatarIndex];
        Debug.Log($"Jugador local index de avatar: {playerAvatarIndex}, Sprite: {playerSprite.name}");

        // Asignar el RuntimeAnimatorController y aplicar el sprite a todos los personajes
        for (int i = 0; i < personajes.Length; i++)
        {
            // Encontrar el transform 'Personaje' en el objeto correspondiente
            Transform personajeTransform = personajes[i].transform.Find("Personaje");

            if (personajeTransform != null)
            {
                // Obtener el Animator del hijo 'Personaje'
                Animator animator = personajeTransform.GetComponent<Animator>();
                if (animator != null)
                {
                    // Asignar el RuntimeAnimatorController basado en el playerAvatarIndex
                    if (playerAvatarIndex < animators.Length)
                    {
                        animator.runtimeAnimatorController = animators[playerAvatarIndex]; // Asigna el controlador basado en el índice del avatar
                        Debug.Log($"RuntimeAnimatorController asignado al Animator del personaje {i}: {animators[playerAvatarIndex].name}");
                    }
                    else
                    {
                        Debug.LogError($"Índice {playerAvatarIndex} fuera de rango para el array animators.");
                    }

                    // Aplicar el sprite
                    SpriteRenderer spriteRenderer = personajeTransform.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = playerSprite;
                        Debug.Log($"Sprite aplicado al personaje {i}: {playerSprite.name}");
                    }
                    else
                    {
                        Debug.LogError($"SpriteRenderer no encontrado en el hijo 'Personaje' del personaje {i}.");
                    }

                    // Aquí puedes activar la animación correspondiente
                    animator.SetInteger("AvatarIndex", playerAvatarIndex);
                    Debug.Log($"Animator configurado para el personaje {i} con AvatarIndex: {playerAvatarIndex}");
                }
                else
                {
                    Debug.LogError($"Animator no encontrado en el hijo 'Personaje' del personaje {i}.");
                }
            }
            else
            {
                Debug.LogError($"Transform 'Personaje' no encontrado para el personaje {i}.");
            }
        }
    }



    public void SpawnJugadores()
    {

        /*
        GameObject playerToSpawn = playerPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];

        if (PhotonNetwork.IsMasterClient)
        {
            // Posiciones específicas para el equipo 1
            Vector3 spawnPosition11 = new Vector3(-17.5f, -1.6f, 0f);   // Sacador
            Vector3 spawnPosition12 = new Vector3(-10.5f, -1.3f, 0f);   // Receptor
            Vector3 spawnPosition13 = new Vector3(-4.5f, -1.3f, 0f);    // Rematador Arriba
            Vector3 spawnPosition14 = new Vector3(-4.5f, -5.6f, 0f);    // Rematador Abajo
            Vector3 spawnPosition15 = new Vector3(-1.4f, -2.6f, 0f);    // Armador

            // Instanciar cada jugador manualmente
            GameObject sacador1 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition11, Quaternion.identity);
            sacador1.GetComponent<ClickHandler>().personajeIndice = 0;
            jugadoresLocales.Add(sacador1);

            GameObject receptor1 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition12, Quaternion.identity);
            receptor1.GetComponent<ClickHandler>().personajeIndice = 1;
            jugadoresLocales.Add(receptor1);

            GameObject rematadorArriba1 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition13, Quaternion.identity);
            rematadorArriba1.GetComponent<ClickHandler>().personajeIndice = 2;
            jugadoresLocales.Add(rematadorArriba1);

            GameObject rematadorAbajo1 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition14, Quaternion.identity);
            rematadorAbajo1.GetComponent<ClickHandler>().personajeIndice = 3;
            jugadoresLocales.Add(rematadorAbajo1);

            GameObject armador1 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition15, Quaternion.identity);
            armador1.GetComponent<ClickHandler>().personajeIndice = 4;
            jugadoresLocales.Add(armador1);
        }
        else
        {
            // Posiciones específicas para el equipo 2
            Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);
            Vector3 spawnPosition21 = new Vector3(13.5f, -4.6f, 0f);   // Sacador
            Vector3 spawnPosition22 = new Vector3(12.5f, -1.3f, 0f);   // Receptor
            Vector3 spawnPosition23 = new Vector3(5.5f, -1.3f, 0f);    // Rematador Arriba
            Vector3 spawnPosition24 = new Vector3(8.5f, -5.6f, 0f);    // Rematador Abajo
            Vector3 spawnPosition25 = new Vector3(3.4f, -3.6f, 0f);    // Armador

            // Instanciar cada jugador manualmente
            GameObject sacador2 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition21, spawnRotation);
            sacador2.GetComponent<ClickHandler>().personajeIndice = 5;
            jugadoresLocales.Add(sacador2);

            GameObject receptor2 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition22, spawnRotation);
            receptor2.GetComponent<ClickHandler>().personajeIndice = 6;
            jugadoresLocales.Add(receptor2);

            GameObject rematadorArriba2 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition23, spawnRotation);
            rematadorArriba2.GetComponent<ClickHandler>().personajeIndice = 7;
            jugadoresLocales.Add(rematadorArriba2);

            GameObject rematadorAbajo2 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition24, spawnRotation);
            rematadorAbajo2.GetComponent<ClickHandler>().personajeIndice = 8;
            jugadoresLocales.Add(rematadorAbajo2);

            GameObject armador2 = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPosition25, spawnRotation);
            armador2.GetComponent<ClickHandler>().personajeIndice = 9;
            jugadoresLocales.Add(armador2);
        }

        List<int> viewIDs = new List<int>();
        foreach (GameObject jugador in jugadoresLocales)
        {
            viewIDs.Add(jugador.GetComponent<PhotonView>().ViewID);
        }

        photonView.RPC("UnirArrays", RpcTarget.All, viewIDs.ToArray());
    }

    [PunRPC]
    public void UnirArrays(int[] viewIDs)
    {
        // Convertir los ViewIDs en GameObjects y añadirlos al array principal
        foreach (int viewID in viewIDs)
        {
            GameObject jugador = PhotonView.Find(viewID).gameObject;
            bool existe = false;

            // Verificar si ya está en el array
            foreach (GameObject existente in personajes)
            {
                if (existente == jugador)
                {
                    existe = true;
                    break;
                }
            }

            // Si no está, añadir al array
            if (!existe)
            {
                List<GameObject> personajesList = new List<GameObject>(personajes);
                personajesList.Add(jugador);
                personajes = personajesList.ToArray();
            }
        }
        */
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

            int personajeMasCercanoId = personajeMasCercano.GetComponent<PhotonView>().ViewID;

            // Verificar si el ID es válido antes de llamar a RecibirPelota
            if (personajeMasCercanoId == 0)
            {
                Debug.LogError("El PhotonView del personaje más cercano tiene un ID 0.");
            }
            else
            {
                RecibirPelota(personajeMasCercanoId);
            }
        }
        else
        {
            Debug.LogError("No se encontró un personaje cercano.");
        }

        return personajeMasCercano;
    }



    [PunRPC]
    void RecibirPelota(int id)
    {
        PhotonView photonview = PhotonView.Find(id);

        if (photonview == null)
        {
            Debug.LogError($"Error en RecibirPelota: No se encontró un PhotonView con el ID {id}");
            return;
        }



        GameObject personajeMasCercano = photonview.gameObject;

        if (personajeMasCercano == null)
        {
            Debug.LogError("Error en RecibirPelota: El objeto personajeMasCercano es nulo.");
            return;
        }

        Debug.LogError($"Recibiendo pelota en el personaje con ID: {id}, Nombre: {personajeMasCercano.name}");

        if (personajeMasCercano != null)
        {
            RecogerPelota(personajeMasCercano);
            
        }
        else
        {
            IsDoingAction = false;
            // Si no hay personajes en rango, ejecutamos la lógica del saque
            if (ball.transform.position.x > 1)
            {
                if (ball.transform.position.x < 18 && ball.transform.position.y < 1 && ball.transform.position.y > -8) 
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP1, textHolder);
                    estado = estado.SaqueP1;
                    InstanciarPersonajesEnPosicionesIniciales();
                    ball.transform.parent = personajes[0].transform;
                    ball.transform.localPosition = new Vector3(7, 12, 0);
                }
                else
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP2,textHolder);
                    estado = estado.SaqueP2;
                    InstanciarPersonajesEnPosicionesIniciales();
                    ball.transform.parent = personajes[5].transform;
                    ball.transform.localPosition = new Vector3(-7, 12, 0);
                }
            }
            else if(ball.transform.position.x < 1)
            {
                if (ball.transform.position.x > -16 && ball.transform.position.y < 1 && ball.transform.position.y > -8) 
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP2,textHolder);
                    estado = estado.SaqueP2;
                    InstanciarPersonajesEnPosicionesIniciales();
                    ball.transform.parent = personajes[5].transform;
                    ball.transform.localPosition = new Vector3(-7, 12, 0);
                }
                else
                {
                    LeantweenScript.AparecerTextoPunto(textoPointP1,textHolder);
                    estado = estado.SaqueP1;
                    InstanciarPersonajesEnPosicionesIniciales();
                    ball.transform.parent = personajes[0].transform;
                    ball.transform.localPosition = new Vector3(7, 12, 0);
                }
            }
            
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

    


    private void InstanciarPersonajesEnPosicionesIniciales()
    {
        if (estado == estado.SaqueP1)
        {
            for (int i = 0; i < personajes.Length && i < posicionesInicialesSaque1.Length; i++)
            {
                Vector3 worldPosition = new Vector3(posicionesInicialesSaque1[i].x + 0.5f, posicionesInicialesSaque1[i].y, 0f);
                personajes[i].transform.position = worldPosition;
                //Debug.Log("Se instanciaron en" + worldPosition);
            }
        }
        else if (estado == estado.SaqueP2)
        {
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
        else if(personajeActual == null && IsDoingAction == false)
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

    //RPCear
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
        
        foreach (GameObject p in personajes)
        {
            Animator otherAnimator = p.GetComponentInChildren<Animator>();
            if (p != personaje)
            {
                // Congelar la animación de los otros personajes
                otherAnimator.speed = 0;
            }
        }

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
        ball.transform.SetParent(null); //mover a la funcion que se photonee
        Sacar();
    }

    public void OnBotonDevolverClick()
    {
        isMovingMode = false;
        ball.transform.SetParent(null);
        Devolver();
    }

    public void OnBotonPasarClick()
    {
        isMovingMode = false;
        ball.transform.SetParent(null);
        Pasar();
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
        ball.transform.SetParent(null);
        Rematar();
    }
    public void OnBotonBloquearClick()
    {
        isMovingMode = false;
        Bloquear();
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

    public void Pasar()
    {
        if(ball.transform.position.x > 1)
        {
            StartCoroutine(SeleccionDePase(ball.transform.position, personajes[9], new Vector3(8,15,0), 2));
        }
        else
        {
            StartCoroutine(SeleccionDePase(ball.transform.position, personajes[4], new Vector3(-8,15,0), -2));
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
        StartCoroutine(SeleccionDeRemate(ball.transform.position));
    }

    public void Bloquear()
    {
        //if (estado == estado.AtaqueP2DefensaP1 && ball.transform.parent == ballHolderAlto)
        StartCoroutine(SeleccionDeBloqueo(personajeActual, personajeActual.transform.position, new Vector3 (personajeActual.transform.position.x, personajeActual.transform.position.y + 1.0f, personajeActual.transform.position.z)));
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
        
        Vector2Int tileAltaP1 = new Vector2Int(-3, 3);
        Vector2Int tileBajaP1 = new Vector2Int(-3, -3);
        
        Vector2Int tileAltaP2 = new Vector2Int(4, 3);
        Vector2Int tileBajaP2 = new Vector2Int(3, -5);
        if(ball.transform.parent == personajes[9].transform)
        {
        //if (personajes[8].transform.position )
          if(casillasPorPosicion.ContainsKey(tileAltaP2))
          {
          casillasPorPosicion[tileAltaP2].SetActive(true);
          }

          if(casillasPorPosicion.ContainsKey(tileBajaP2))
          {
          casillasPorPosicion[tileBajaP2].SetActive(true);
          }
        }
        else if(ball.transform.parent == personajes[4].transform)
        {
          if(casillasPorPosicion.ContainsKey(tileAltaP1))
          {
          casillasPorPosicion[tileAltaP1].SetActive(true);
          }

          if(casillasPorPosicion.ContainsKey(tileBajaP1))
          {
          casillasPorPosicion[tileBajaP1].SetActive(true);
          }
        }
    }



    /*
     Crear una funcion aparte que tire las posciciones finales y llame a recibir pelota (esto porque Photon no puede pasar IEnumerators)
    Cosas a pasar en la funcion alternativa:
    - El jugador saco (para los estados)
    - El jugador del otro equipo recibio o no
     */
    private IEnumerator SeleccionDeSaque(Vector3 start, GameObject Sacador)
    {
        IsDoingAction = true;
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
                    List<Vector2Int> CasillasPosibles = GetReachableTiles(gridPosition, 3);
                    int randomIndex = UnityEngine.Random.Range(0, CasillasPosibles.Count);
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

        Debug.LogError(personajeMasCercano.name);

        // Mover la pelota a la casilla seleccionada
        float duration = 1.5f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x + 0.5f, casillaObjetivo.y + 0.5f, 0);

        Debug.Log($"Posición inicial: {startPosition}, Posición final: {endPosition}");

        // Determina la altura máxima de la parábola
        float heightMax = 2.5f;

        DesactivarCasillasIluminadas();
        if (Sacador.transform.position.x > 0)
        {
            ball.GetComponent<Animator>().SetBool("IsGoingRight", true);
        }
        else
        {
            ball.GetComponent<Animator>().SetBool("IsGoingLeft", true);
        }

        while (elapsedTime < duration)
        {
            DeactivateAllButtons();

            // Interpolación en el eje X e Y
            float t = elapsedTime / duration;

            // Interpolación en X e Y con una parábola en Z para la altura
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += Mathf.Sin(t * Mathf.PI) * heightMax;

            //Debug para ver la poscicion de la pelota
            //Debug.Log($"Tiempo: {elapsedTime}, Posición de la pelota: {currentPos}, t: {t}");

            ball.transform.position = currentPos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ball.GetComponent<Animator>().SetBool("IsGoingLeft", false);
        ball.GetComponent<Animator>().SetBool("IsGoingRight", false);

        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;
        Debug.Log($"Pelota llegó a la posición final: {endPosition}");

        // Enviar el id del personaje más cercano para que reciba la pelota
        int personajeMasCercanoId = personajeMasCercano.GetComponent<PhotonView>().ViewID;

        Debug.LogError(personajeMasCercanoId);

        photonView.RPC("RecibirPelota", RpcTarget.All, personajeMasCercanoId);

        if (estadoActual != estado)
        {
            displayPuntosScript.SumarTurnoDisplay();
        }

        yield return new WaitForSeconds(1.2f);
        IsDoingAction = false;
    }
    private IEnumerator SeleccionDeDevolver(Vector3 start)
    {
        IsDoingAction = true;
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
                    List<Vector2Int> CasillasPosibles = GetReachableTiles(gridPosition, 3);
                    int randomIndex = UnityEngine.Random.Range(0, CasillasPosibles.Count);
                    casillaObjetivo = CasillasPosibles[randomIndex];
                    casillaSeleccionada = true;
                }
            }

            yield return null;
        }

        // Mover la pelota a la casilla seleccionada
        DescongelarAnimaciones();
        float duration = 2.5f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x + 0.5f, casillaObjetivo.y + 0.5f, 0);
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("endreceive");
        personajeActual.GetComponentInChildren<Animator>().SetBool("receive", false);
        float heightMax = 4.0f;

        DesactivarCasillasIluminadas();
        GameObject personajeMasCercano = IrARecogerPelota(casillaObjetivo);

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
         Vector3 casillaObjetivoV3 = new Vector3(casillaObjetivo.x, casillaObjetivo.y, 0);
        int personajeMasCercanoId = personajeMasCercano.GetComponent<PhotonView>().ViewID;
        RecibirPelota(personajeMasCercanoId);

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
        //Debug.Log($"Tiempo: {elapsedTime}, Posición de la pelota: {currentPos}, t: {t}");

        // Asignar la nueva posición
        ball.transform.position = currentPos;

        elapsedTime += Time.deltaTime;
        yield return null;
    }
        ball.transform.parent = armador.transform;
        ball.transform.localPosition = posicionArmadoDePelota;
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
    }
    
private IEnumerator SeleccionDeArmado(Vector3 start)
{
        Vector2Int tileAltaP1 = new Vector2Int(-3, 3);
        Vector2Int tileBajaP1 = new Vector2Int(-3, -4);
        
        Vector2Int tileAltaP2 = new Vector2Int(3, 3);
        Vector2Int tileBajaP2 = new Vector2Int(3, -5);

        ball.transform.SetParent(null);
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
        personajeActual.GetComponentInChildren<SpriteRenderer>().flipX = !personajeActual.GetComponentInChildren<SpriteRenderer>().flipX;
        if (ball.transform.position.y > 2 )
        {
            ball.transform.parent = ballHolderAlto.transform;
        }
        else if(ball.transform.position.y < -2 )
        {
            ball.transform.parent = ballHolderBajo.transform;
        }
        IsDoingAction = false;
    }
        //Vector2 Skibidi.Transform
        //Console.Lenght mewing;

    public IEnumerator SeleccionDeRemate(Vector3 start)
    {
        IsDoingAction = true;
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
                    List<Vector2Int> CasillasPosibles = GetReachableTiles(gridPosition, 2);
                    int randomIndex = UnityEngine.Random.Range(0, CasillasPosibles.Count);
                    casillaObjetivo = CasillasPosibles[randomIndex];
                    casillaSeleccionada = true;
                }
            }

            yield return null;
        }
        Vector3 casillaObjetivoV3 = new Vector3(casillaObjetivo.x, casillaObjetivo.y, 0);
        DescongelarAnimaciones();
        if (IsBlocking == true)
        {
           /* float probabilidadDeBloqueo = Random.Range(0f, 1f);
            Debug.Log(probabilidadDeBloqueo);
            if(probabilidadDeBloqueo > 1f)
            {*/
                StartCoroutine(PelotaBloqueada());
           // }
        }
        else
        {
        //if(pro)
        // Mover la pelota a la casilla seleccionada
        float duration = 1.0f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x + 0.5f, casillaObjetivo.y + 0.5f, 0);
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("Spike");
        yield return new WaitForSeconds(0.7f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.12f);
        Time.timeScale = 1.0f;
        LeantweenScript.AparecerTextoPunto(textoSpike, textHolderPopUpLeft);
        GameObject personajeMasCercano = IrARecogerPelota(casillaObjetivo);
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            Quaternion startRotation = ball.transform.rotation; // rotación inicial
                Quaternion endRotation = Quaternion.Euler(0, 360, 0); // rotación final (ejemplo de 360 grados en Y)
                ball.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
                yield return null;
        }

        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;

            int personajeMasCercanoId = personajeMasCercano.GetComponent<PhotonView>().ViewID;
            RecibirPelota(personajeMasCercanoId);
        }

        if (estadoActual != estado)
        {
            displayPuntosScript.SumarTurnoDisplay();
        }
        IsDoingAction = false;
    }

    private IEnumerator SeleccionDeBloqueo(GameObject personaje, Vector3 start, Vector3 end)
    {
        personajeBloqueando = personajeActual;
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
        // Verifica que el personaje tiene al menos tres hijos
        if (personaje.transform.childCount > 2)
        {
            Vector2Int ballPosition = casillaObjetivo;
            Vector2Int personajePosition = GetGridPosition(personaje.transform.GetChild(2).position);

            return Vector2Int.Distance(personajePosition, ballPosition);
        }
        else
        {
            Debug.LogError($"El personaje {personaje.name} no tiene suficientes hijos. childCount: {personaje.transform.childCount}");
            return float.MaxValue; // Devolvemos un valor alto para indicar que no se puede calcular la distancia
        }
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

    public IEnumerator PelotaBloqueada()
    {
        Debug.Log("PELOTA BLOQUEADA");
        DescongelarAnimaciones();
        float duration = 0.5f;
        float elapsedTime = 0;

        Vector3 startPosition = ball.transform.position;
        Vector3 endPosition = new Vector3(personajeBloqueando.transform.position.x + personajeBloqueando.transform.position.y + 2.0f,  personajeBloqueando.transform.position.z);
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
    }

    
}