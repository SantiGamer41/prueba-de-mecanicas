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
    float tileSize = 1.0f;

    void Start()
    {
        
        // Desactivar el botón al inicio
        DesactivarCasillasIluminadas();
        botonMoverPersonaje1.gameObject.SetActive(false);
        maxMovementRange = 2; // Ejemplo de rango máximo de movimiento
    }

    public void SeleccionarPersonaje(int indice)
    {
        if (indice >= 0 && indice < personajes.Length)
        {
            personajeActual = personajes[indice];
            ObtenerUbicacionDelPersonaje();
        }
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void ActivarBotonMoverPersonaje()
    {
        // Activar el GameObject asociado al botón de mover personaje
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

        // Obtener los tiles alcanzables utilizando startTile y maxMovementRange
        List<Vector2Int> reachableTiles = GetReachableTiles(startTile, maxMovementRange);

        // Instanciar las casillas iluminadas en las posiciones alcanzables
        foreach (var tile in reachableTiles)
        {
            // Calcular la posición en el mundo a partir de la posición de la grid
            Vector3 worldPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, 0f);

            // Instanciar el prefab de la casilla iluminada en la posición calculada
            GameObject casilla = Instantiate(casillaIluminadaPrefab, worldPosition, Quaternion.identity);
            casilla.tag = "Iluminada";
            Vector3Int gridPosition = new Vector3Int(tile.x, tile.y, 0);
            Debug.Log("Reachable tiles count: " + reachableTiles.Count);

            //mostrar los tiles del otro tilemap


        }

        // Mostrar los valores de startTile y maxMovementRange en la consola para verificar
        Debug.Log("startTile: " + startTile);
        Debug.Log("maxMovementRange: " + maxMovementRange);
    }
    public void SetTilesAlpha()
    {

    }

    public void MoverPersonajeA(Vector3 nuevaPosicion)
    {
        if (personajeActual != null)
        {
            // Mover el personaje a la nueva posición
            personajeActual.transform.position = nuevaPosicion;

            // Opcional: Desactivar las casillas iluminadas después de mover
            DesactivarCasillasIluminadas();
            botonMoverPersonaje1.gameObject.SetActive(false);
        }
    }

    private void DesactivarCasillasIluminadas()
    {
        // Encuentra todos los objetos instanciados de casillas iluminadas y destrúyelos
        GameObject[] casillas = GameObject.FindGameObjectsWithTag("Iluminada");
        foreach (GameObject casilla in casillas)
        {
            // Destruye cada objeto casilla iluminada
            Destroy(casilla);
        }
    }
    // Función para verificar si una tile es accesible
    bool IsTileAccessible(Vector2Int tile)
    {
        // Aquí puedes definir la lógica para determinar si una tile es accesible
        // Por ejemplo, podrías comparar con una lista de tiles no accesibles
        List<Vector2Int> nonAccessibleTiles = new List<Vector2Int>
    {
        new Vector2Int(-17, -7),
        new Vector2Int(19, -7),

        // Agrega más coordenadas no accesibles aquí
    };

        return !nonAccessibleTiles.Contains(tile);
    }
    // Función para obtener los tiles alcanzables dentro de un rango específico desde un tile inicial
    List<Vector2Int> GetReachableTiles(Vector2Int startTile, int maxMovementRange)
    {
        List<Vector2Int> reachableTiles = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startTile);
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        distances[startTile] = 0;

        // Direcciones de movimiento: arriba, abajo, izquierda, derecha
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

            // Explorar los vecinos en las 4 direcciones
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = currentTile + dir;

                // Verificar si el vecino no ha sido visitado, está dentro del rango máximo, y es accesible
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
/*
   List<Vector2Int> GetReachableTiles(Vector2Int startTile, int maxMovementRange)
    {
        List<Vector2Int> reachableTiles = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startTile);
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        distances[startTile] = 0;

        // Direcciones de movimiento: arriba, abajo, izquierda, derecha
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

            // Explorar los vecinos en las 4 direcciones
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = currentTile + dir;

                // Verificar si el vecino no ha sido visitado y está dentro del rango máximo
                if (!distances.ContainsKey(neighbor))
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
 */