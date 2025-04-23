using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject inputPrefab;   // Prefab for input nodes (A and B)
    public GameObject outputPrefab;  // Prefab for output node

    public int rows = 5;
    public int columns = 8;
    public float spacing = 1.5f;
    public Vector2 cameraCenter = new Vector2(5f, 3f);

    public Vector2 inputAPosition = new Vector2(1, 1);
    public Vector2 inputBPosition = new Vector2(2, 2);
    public Vector2 outputPosition = new Vector2(6, 3); // Position for the Output node

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
        PlaceInputAt(inputAPosition, "Input A");
        PlaceInputAt(inputBPosition, "Input B");
    }

    void PlaceInputAt(Vector2 gridPosition, string inputName)
    {
        Vector2 gridOffset = new Vector2(columns / 2f, rows / 2f);
        Vector2 worldPos = (gridPosition - gridOffset + cameraCenter) * spacing;

        GameObject inputNode = Instantiate(inputPrefab, worldPos, Quaternion.identity, this.transform);
        InputNode inputNodeScript = inputNode.GetComponent<InputNode>();
    }

    void PlaceOutputs()
    {
        Vector2 gridOffset = new Vector2(columns / 2f, rows / 2f);
        Vector2 worldPos = (outputPosition - gridOffset + cameraCenter) * spacing;

        GameObject outputNode = Instantiate(outputPrefab, worldPos, Quaternion.identity, this.transform);
        outputNode.name = "Output Node";
    }
}
