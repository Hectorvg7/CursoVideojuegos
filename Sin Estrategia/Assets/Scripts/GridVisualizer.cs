using UnityEngine;
using UnityEngine.AI;

public class GridVisualizer : MonoBehaviour
{
    public static GridVisualizer Instance { get; private set; }
    public GameObject validMoveColor;
    public GameObject invalidMoveColor;
    public float alturaCasilla = 0.2f;
    public Quaternion rotacionCasilla = Quaternion.Euler(90f, 0f, 0f);

    private GridSystem gridSystem;
    private Unit selectedUnit; // La unidad seleccionada

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Hay más de una instancia de GridVisualizer.");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gridSystem = LevelGrid.Instance.gridSystem;
    }

    void OnDrawGizmos()
    {
        if (gridSystem == null || selectedUnit == null)
        {
            return;
        }

        // Limpiar cualquier objeto instanciado previamente (opcional)
        BorrarQuads();

        // Obtenemos el rango de movimiento de la unidad seleccionada
        int moveRange = selectedUnit.maxPointsPerTurn; // Supongamos que el rango es igual a los puntos de movimiento

        // Iteramos por la rejilla y dibujamos solo las celdas dentro del rango de movimiento
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = gridSystem.GetGridObject(gridPosition);

                if (gridObject == null) continue;

                // Comprobar si la celda está dentro del rango de movimiento
                int distance = Mathf.Abs(gridPosition.x - selectedUnit.GetGridPosition().x) + Mathf.Abs(gridPosition.z - selectedUnit.GetGridPosition().z);
                bool isValidMove = distance <= moveRange;

                GameObject casilla = isValidMove ? validMoveColor : invalidMoveColor;

                // Instanciar el prefab en la posición correspondiente
                Vector3 position = LevelGrid.Instance.GetWorldPosition(gridPosition) + new Vector3(0f, alturaCasilla, 0f);
                if (casilla != null)
                {
                    Instantiate(casilla, position, rotacionCasilla);
                }
            }
        }
    }

    private void BorrarQuads()
    {
        GameObject[] antiguosQuads = GameObject.FindGameObjectsWithTag("MoveQuad");

        foreach (var quad in antiguosQuads)
        {
            Destroy(quad);
        }
    }

    // Llamado para actualizar la unidad seleccionada
    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
    }
}

