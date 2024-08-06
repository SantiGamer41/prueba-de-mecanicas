using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class gameManager : MonoBehaviour
{
    public Tilemap canchaIluminadaTilemap; // Referencia al Tilemap "canchailuminada"
    public Color colorToApply;
    public GameObject[] personajes; // Array de personajes
    private GameObject personajeActual; // Personaje actualmente seleccionado
    public Button botonMoverPersonaje1; // Referencia al botón en Unity
    public GameObject casillaIluminadaPrefab;
    private Vector2Int startTile;
    private int maxMovementRange;
    private bool IsMoving;
    public Animator animator;
    private Vector2Int[] posicionesIniciales = new Vector2Int[]
  {
        new Vector2Int(-18, -6), 
        new Vector2Int(-11, -1),
        new Vector2Int(-5, -1),
        new Vector2Int(-5, -6),
        new Vector2Int(-2, -3)
  };


    // Diccionario para almacenar las casillas por su posición
    private Dictionary<Vector2Int, GameObject> casillasPorPosicion = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        IsMoving = false;
        InstanciarCasillas();
        DesactivarCasillasIluminadas();
        botonMoverPersonaje1.gameObject.SetActive(false);
        maxMovementRange = 2; // Ejemplo de rango máximo de movimiento
        InstanciarPersonajesEnPosicionesIniciales();
    }

    private void InstanciarCasillas()
    {
        for (int x = -17; x <= 19; x++)
        {
            for (int y = -9; y <= 1; y++)
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
            personajeActual = personajes[indice];
            ObtenerUbicacionDelPersonaje();
            animator = personajeActual.GetComponentInChildren<Animator>();
        }
    }

    public void ActivarBotonMoverPersonaje()
    {
        botonMoverPersonaje1.gameObject.SetActive(true);
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

    public void OnButtonClick()
    {
        ObtenerUbicacionDelPersonaje();
        MostrarCasillasAlcanzables();
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

    public void MoverPersonajeA(Vector3 nuevaPosicion)
    {
        if (personajeActual != null)
        {
            StartCoroutine(MovimientoPersonaje(personajeActual.transform.position, nuevaPosicion));
            personajeActual.transform.position = nuevaPosicion;
            DesactivarCasillasIluminadas();
            botonMoverPersonaje1.gameObject.SetActive(false);
        }
    }
    private IEnumerator MovimientoPersonaje(Vector3 start, Vector3 end)
    {
        float duration = 2.0f; // Duración de la animación
        float elapsedTime = 0;
        IsMoving = true;
        Vector3 startPosition = new Vector3(start.x + 0.5f, start.y + 0.5f, start.z); // Ajustar la posición inicial
        Vector3 endPosition = new Vector3(end.x + 0.5f, end.y + 0.5f, end.z); // Ajustar la posición final
        while (elapsedTime < duration)
        {
            animator.SetBool("IsMoving", true);
            personajeActual.transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IsMoving = false;
        Debug.Log(start);
        Debug.Log(end);
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

}