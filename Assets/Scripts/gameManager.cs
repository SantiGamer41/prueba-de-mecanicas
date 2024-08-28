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
    [Header("Personajes")]
    public GameObject[] personajes; // Array de personajes
    [Space(25)]
    [HideInInspector]
    public GameObject personajeActual; // Personaje actualmente seleccionado
    [Header("Objetos")]
    public GameObject ball; //Pelota
    public GameObject flecha; 
    public GameObject casillaIluminadaPrefab;
    private int maxMovementRange;
    private float ballPickupRange = 3.0f;
    private bool IsServing;
    [Space(25)]
    private bool isMovingMode = false; 
    [Header("Botónes")]
    public Button botonMoverPersonaje; // Referencia al botón en Unity
    public Button botonSacar;
    public Button botonDevolver;
    [Space(25)]
    private Vector2Int startTile;
    [Header("Animator")]
    public Animator animator;
    private Vector2Int[] posicionesIniciales = new Vector2Int[]
  {
        new Vector2Int(-18, -6),
        new Vector2Int(-11, -1),
        new Vector2Int(-5, -1),
        new Vector2Int(-5, -6),
        new Vector2Int(-2, -3),
        new Vector2Int(13, -5),
        new Vector2Int(12, -1),
        new Vector2Int(8, -6),
        new Vector2Int(5, -1),
        new Vector2Int(3, -4)

  };


    // Diccionario para almacenar las casillas por su posición
    private Dictionary<Vector2Int, GameObject> casillasPorPosicion = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        estado = estado.SaqueP1;
        flecha.SetActive(false);
        InstanciarCasillas();
        DesactivarCasillasIluminadas();
        botonMoverPersonaje.gameObject.SetActive(false);
        botonSacar.gameObject.SetActive(false);
        botonDevolver.gameObject.SetActive(false);
        maxMovementRange = 2; // Ejemplo de rango máximo de movimiento
        InstanciarPersonajesEnPosicionesIniciales();
    }
    public void OnEstadoChange()
    {

    }
    void RecibirPelota()
    {
        bool enRango = false;
        int personajeIindex = 0;
        while(!enRango && personajeIindex <personajes.Length)
        { 
            
            enRango = IntentarRecogerPelota(personajes[personajeIindex]);
            personajeIindex++;
        }        
        if (!enRango)
        {
            //punto
        }
        else
        {
            

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
        for (int i = 0; i < personajes.Length && i < posicionesIniciales.Length; i++)
        {
            Vector3 worldPosition = new Vector3(posicionesIniciales[i].x + 0.5f , posicionesIniciales[i].y, 0f);
            personajes[i].transform.position = worldPosition;
            //Debug.Log("Se instanciaron en" + worldPosition);
        }
    }


    public void SeleccionarPersonaje(int indice)
    {
    if (indice >= 0 && indice < personajes.Length)
        {
            DestroyImmediate(flecha, true);
            personajeActual = personajes[indice];
            ObtenerUbicacionDelPersonaje();
            animator = personajeActual.GetComponentInChildren<Animator>();
            Instantiate(flecha);
            flecha.transform.SetParent(personajeActual.transform);
            flecha.transform.localPosition = new Vector3(0.2f, 6.0f, 0);
            flecha.SetActive(true);
            

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
     private bool IntentarRecogerPelota(GameObject personaje)
    {
        if (personaje != null && ball != null && ball.transform.parent == null)
        {
            
            Vector2Int ballPosition = GetGridPosition(ball.transform.position);
            Vector2Int personajePosition = GetGridPosition(personaje.transform.position);

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
    
    public void MoverPersonajeA(Vector3 nuevaPosicion)
    {
        if (personajeActual != null && isMovingMode)
        {
            StartCoroutine(MovimientoPersonaje(personajeActual.transform.position, nuevaPosicion));
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


    public void MostrarCasillasAlcanzables()
    {
        DesactivarCasillasIluminadas(); // Asegurarse de ocultar todas las casillas primero

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
                casillaObjetivo = gridPosition;
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
        ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Asegura que la pelota termine exactamente en la posición final
    ball.transform.position = endPosition;
        RecibirPelota();
    if (endPosition.x < 1 )
    {
        estado = estado.AtaqueP1DefensaP2;

    }
    else
    {
        estado = estado.AtaqueP2DefensaP1;
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
                    casillaObjetivo = gridPosition;
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
        while (elapsedTime < duration)
        {
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que la pelota termine exactamente en la posición final
        ball.transform.position = endPosition;
        RecibirPelota();
        if (endPosition.x < 1)
        {
            estado = estado.AtaqueP1DefensaP2;

        }
        else
        {
            estado = estado.AtaqueP2DefensaP1;
        }
    }
    //}


    // Genera el rango de caída y mueve la pelota
    //GenerarRangoDeCaida(casillaObjetivo);


    /*private void GenerarRangoDeCaida(Vector2Int casillaObjetivo)
    {
        // Definimos un rango alrededor de la casilla objetivo
        int rangoCaida = 1; // Puedes ajustar este valor según tus necesidades

        List<Vector2Int> posiblesCaidas = new List<Vector2Int>();

        for (int x = -rangoCaida; x <= rangoCaida; x++)
        {
            for (int y = -rangoCaida; y <= rangoCaida; y++)
            {
                Vector2Int posibleCaida = casillaObjetivo + new Vector2Int(x, y);
                if (casillasPorPosicion.ContainsKey(posibleCaida))
                {
                    posiblesCaidas.Add(posibleCaida);
                }
            }
        }

        // Selecciona una casilla al azar dentro del rango donde caerá la pelota
        Vector2Int casillaFinal = posiblesCaidas[Random.Range(0, posiblesCaidas.Count)];

        // Mueve la pelota a la casilla seleccionada
        MoverPelotaA(new Vector3(casillaFinal.x + 0.5f, casillaFinal.y + 0.5f, 0f));
    }
    */


    public IEnumerator MovimientoPersonaje(Vector3 start, Vector3 end)
    {
        float duration = 1.0f; // Duración de la animación
        float elapsedTime = 0;

        // Ajusta la posición inicial al centro de la casilla
        Vector3 startPosition = new Vector3(start.x, start.y + 0.5f, start.z);
        Vector3 endPosition = new Vector3(end.x, end.y -0.5f, end.z); // end ya está ajustado en MoverPersonajeA

        // Imprime las posiciones ajustadas
        Debug.Log("Start (ajustado): " + startPosition);
        Debug.Log("End (ajustado): " + endPosition);

        while (elapsedTime < duration)
        {
            animator.SetBool("IsMoving", true);
            personajeActual.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        personajeActual.transform.position = endPosition; // Asegurar que el personaje termine exactamente en la posición final
        animator.SetBool("IsMoving", false);
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
}