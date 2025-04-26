using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject inputPrefab;
    public GameObject outputPrefab;

    public int rows = 5;
    public int columns = 8;
    public float spacing = 1.5f;
    public Vector2 cameraCenter = new Vector2(5f, 3f);

    [Header("Dynamic Input/Output")]
    public int numberOfInputs = 2;
    public int numberOfOutputs = 1;

    public List<Vector2> inputPositions = new List<Vector2>();
    public List<Vector2> outputPositions = new List<Vector2>();

    void Start()
    {
        GenerateGrid();
        PlaceInputs();
        PlaceOutputs();
    }

    void GenerateGrid()
    {
        Vector2 gridOffset = new Vector2(columns / 2f, rows / 2f);

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 pos = new Vector2(x, y) - gridOffset + cameraCenter;
                GameObject tile = Instantiate(tilePrefab, pos * spacing, Quaternion.identity, this.transform);

                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.gridX = x;
                tileScript.gridY = y;
            }
        }
    }

    void PlaceInputs()
    {
        for (int i = 0; i < numberOfInputs && i < inputPositions.Count; i++)
        {
            PlaceNodeAt(inputPrefab, inputPositions[i], $"Input {i + 1}");
        }
    }

    void PlaceOutputs()
    {
        for (int i = 0; i < numberOfOutputs && i < outputPositions.Count; i++)
        {
            PlaceNodeAt(outputPrefab, outputPositions[i], $"Output {i + 1}");
        }
    }

    void PlaceNodeAt(GameObject prefab, Vector2 gridPosition, string nodeName)
    {
        Vector2 gridOffset = new Vector2(columns / 2f, rows / 2f);
        Vector2 worldPos = (gridPosition - gridOffset + cameraCenter) * spacing;

        GameObject node = Instantiate(prefab, worldPos, Quaternion.identity, this.transform);
        node.name = nodeName;
    }
}
