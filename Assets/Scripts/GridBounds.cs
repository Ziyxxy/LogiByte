using UnityEngine;

public class GridBounds : MonoBehaviour
{
    public static GridBounds Instance;

    public Vector2 gridSize = new Vector2(5, 8);
    public Vector2 tileSpacing = new Vector2(1.5f, 1.5f);
    public Vector3 gridOrigin = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsInsideBounds(Vector3 position)
    {
        Vector2 worldSize = new Vector2(gridSize.x * tileSpacing.x, gridSize.y * tileSpacing.y);
        Vector3 min = gridOrigin;
        Vector3 max = gridOrigin + new Vector3(worldSize.x, worldSize.y, 0);

        return position.x >= min.x && position.x <= max.x &&
               position.y >= min.y && position.y <= max.y;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 realSize = new Vector2(gridSize.x * tileSpacing.x, gridSize.y * tileSpacing.y);
        Vector3 center = gridOrigin + new Vector3(realSize.x / 2f, realSize.y / 2f, 0);
        Gizmos.DrawWireCube(center, new Vector3(realSize.x, realSize.y, 0.1f));
    }
}
