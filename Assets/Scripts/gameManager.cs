using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public enum estado
{
SaqueP1,
SaqueP2,
AtaqueP1DefensaP2,
AtaqueP2DefensaP1,
}
public class gameManager : MonoBehaviour
{

    public estado estado;
    [Space(25)]
    [Header("UI")]
    private estado estadoActual;
    public int turno;
    public Text txt_Turno;
    [Space(25)]
    [Header("Personajes")]
    public GameObject[] personajes; // Array de personajes
    [Space(25)]
    [HideInInspector]
    public GameObject personajeActual; // Personaje actualmente seleccionado
    [Header("Objetos")]
    public GameObject ball; //Pelota
    public GameObject casillaIluminadaPrefab;
    public GameObject ballHolderAlto;
    public GameObject ballHolderBajo;
    private int maxMovementRange;
    public float ballPickupRange = 2.5f;
    private bool enRango = false;
    private bool IsServing;
    [Space(25)]
    private bool isMovingMode = false;
    [Header("Botónes")]
    public GameObject[] botones;
    public Button botonMoverPersonaje; // Referencia al botón en Unity
    public Button botonSacar;
    public Button botonDevolver;
    public Button botonPasar;
    public Button botonArmar;
    [Space(25)]
    private Vector2Int startTile;
    [Header("Animator")]
    public Animator animator;
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
        InstanciarCasillas();
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        maxMovementRange = 2; // Ejemplo de rango máximo de movimiento
        InstanciarPersonajesEnPosicionesIniciales();
        MostrarOpcionesDeArmar();
    }
    public void OnEstadoChange()
    {

    }
     void RecibirPelota()
    {
        enRango = false;
        int personajeIindex = 0;
        while(!enRango && personajeIindex <personajes.Length)
        { 
            
            enRango = IntentarRecogerPelota(personajes[personajeIindex]);
            personajeIindex++;
        }        
        if (enRango == false)
        {
            //Sumar punto Añadir logica de cambiar a estado AtaqueP2 y ataque p1 a este if asi no se contradicen
            if (ball.transform.position.x > 1)
            {
                estado = estado.SaqueP1;
                InstanciarPersonajesEnPosicionesIniciales();
                ball.transform.parent = personajes[0].transform;
                ball.transform.localPosition = new Vector3(7,12, 0);
            }
            else
            {
                estado = estado.SaqueP2;
                InstanciarPersonajesEnPosicionesIniciales();
                ball.transform.parent = personajes[5].transform;
                ball.transform.localPosition = new Vector3(-7,12, 0);
            }


        }
        else
        {
            if (ball.transform.position.x < 1)
        {
            estado = estado.AtaqueP1DefensaP2;

        }
        else
        {
            estado = estado.AtaqueP2DefensaP1;
        }
            

        }
    }
        private void InstanciarCasillas()
    {
        for (int x = -17; x <= 19; x++)
        {
            for (int y = -7; y <= 1; y++)
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
        if (personajeActual != null)
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
        else
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
                animator.SetTrigger("receive");
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
        ball.transform.SetParent(null);
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
    }
    public void OnBotonArmarClick()
    {
        isMovingMode = false;
        ball.transform.SetParent(null);
        Armar();
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
      StartCoroutine(SeleccionDeSaque(ball.transform.position));
    
     }

    public void Devolver()
    {
        MostrarLadoContrario();
        StartCoroutine(SeleccionDeRemate(ball.transform.position));
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
        
        Vector2Int tileAltaP1 = new Vector2Int(-3, 1);
        Vector2Int tileBajaP1 = new Vector2Int(-3, -6);
        
        Vector2Int tileAltaP2 = new Vector2Int(3, 1);
        Vector2Int tileBajaP2 = new Vector2Int(3, -6);
        if(ball.transform.parent == personajes[9].transform)
        {
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

   
private IEnumerator SeleccionDeSaque(Vector3 start)
{
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
               int randomIndex = Random.Range(0, CasillasPosibles.Count);
               casillaObjetivo = CasillasPosibles[randomIndex];
               casillaSeleccionada = true;
            }
        }

        yield return null;
    }

    // Mover la pelota a la casilla seleccionada
    float duration = 3.0f;
    float elapsedTime = 0;

    Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
    Vector3 endPosition = new Vector3(casillaObjetivo.x + 0.5f, casillaObjetivo.y + 0.5f, 0);
    DesactivarCasillasIluminadas();
    while (elapsedTime < duration)
    {
        DeactivateAllButtons();
        ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Asegura que la pelota termine exactamente en la posición final
    ball.transform.position = endPosition;
        RecibirPelota();
    
    
    if (endPosition.x < 1 && enRango == true)
    {
        
        StartCoroutine(MovimientoPersonaje(personajes[5].transform.position, new Vector3(12.5f, -0.5f, 0), personajes[5]));
    }
    else if (endPosition.x > -1 && enRango == true)
    {
      
        StartCoroutine(MovimientoPersonaje(personajes[0].transform.position, new Vector3(-12.5f, -6.5f, 0), personajes[0]));
    }   
    
    if (estadoActual != estado)
    {
            turno++;
            txt_Turno.text =  "Turno " +turno.ToString();
    }
    }
private IEnumerator SeleccionDeRemate(Vector3 start)
    {
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
                    int randomIndex = Random.Range(0, CasillasPosibles.Count);
                    casillaObjetivo = CasillasPosibles[randomIndex];
                    casillaSeleccionada = true;
                }
            }

            yield return null;
        }

        // Mover la pelota a la casilla seleccionada
        float duration = 2.0f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x + 0.5f, casillaObjetivo.y + 0.5f, 0);
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;
        RecibirPelota();
        
        if(estadoActual != estado)
        {
            turno++;
            txt_Turno.text = "Turno " + turno.ToString();
        }
    }
private IEnumerator SeleccionDePase(Vector3 start, GameObject armador, Vector3 posicionArmadoDePelota, int correccionDePase)
{
        float duration = 1.0f; // Duración de la animación
        float elapsedTime = 0;

        // Ajusta la posición inicial al centro de la casilla
        Vector3 startPosition = new Vector3(start.x, start.y, start.z);
        Vector3 endPosition = new Vector3(armador.transform.position.x + correccionDePase, armador.transform.position.y + 3, armador.transform.position.z); // end ya está ajustado en MoverPersonajeA

        // Imprime las posiciones ajustadas
        DeactivateAllButtons();
        personajeActual.GetComponentInChildren<Animator>().SetTrigger("Pass");
        armador.GetComponentInChildren<SpriteRenderer>().flipX = !armador.GetComponentInChildren<SpriteRenderer>().flipX;
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ball.transform.parent = armador.transform;
        ball.transform.localPosition = posicionArmadoDePelota;
}
    
private IEnumerator SeleccionDeArmado(Vector3 start)
{
        Vector3 tileAltaP1 = new Vector3(-3, 1, 0);
        Vector3 tileBajaP1 = new Vector3(-3, -6, 0);
        
        Vector3 tileAltaP2 = new Vector3(3, 1, 0);
        Vector3 tileBajaP2 = new Vector3(3, -6, 0);

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
            }

            yield return null;
        }

        // Mover la pelota a la casilla seleccionada
        float duration = 1.0f;
        float elapsedTime = 0;

        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(casillaObjetivo.x + 0.5f, casillaObjetivo.y + 0.5f, 0);
        DesactivarCasillasIluminadas();
        DeactivateAllButtons();
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if(ball.transform.position == tileAltaP1 || ball.transform.position == tileAltaP2)
        {
            ball.transform.parent = ballHolderAlto.transform;
        }
        else if(ball.transform.position == tileBajaP1 || ball.transform.position == tileBajaP2)
        {
            ball.transform.parent = ballHolderBajo.transform;
        }
}
    public IEnumerator MovimientoPersonaje(Vector3 start, Vector3 end, GameObject personaje)
    {
        Debug.Log(personaje);
        float duration = 1.0f; // Duración de la animación
        float elapsedTime = 0;

        // Ajusta la posición inicial al centro de la casilla
        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(end.x, end.y -0.5f, end.z); // end ya está ajustado en MoverPersonajeA

        // Imprime las posiciones ajustadas
        Debug.Log("Start (ajustado): " + startPosition);
        Debug.Log("End (ajustado): " + endPosition);
        DeactivateAllButtons();
        while (elapsedTime < duration)
        {
            DeactivateAllButtons();
            personaje.GetComponentInChildren<Animator>().SetBool("IsMoving", true);
            personaje.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        personajeActual.transform.position = endPosition; // Asegurar que el personaje termine exactamente en la posición final
        personaje.GetComponentInChildren<Animator>().SetBool("IsMoving", false);
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
}