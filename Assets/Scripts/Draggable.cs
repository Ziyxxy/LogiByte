using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera cam;
    private Tile nearestTile;
    private Vector3 originalPosition;
    private bool isFirstDrop = true;
    private Tile currentHoveredTile;  // Declare this to track the currently hovered tile

    void Start()
    {
        cam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            return;

        if (cam == null)
            cam = Camera.main;

        offset = transform.position - cam.ScreenToWorldPoint(eventData.position);
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            return;

        if (cam == null)
            cam = Camera.main;

        Vector3 mousePos = cam.ScreenToWorldPoint(eventData.position) + offset;
        mousePos.z = 0;
        transform.position = mousePos;

        Tile hovered = FindClosestTile();

        float highlightRange = 0.6f;
        if (hovered != null && Vector2.Distance(transform.position, hovered.transform.position) <= highlightRange)
        {
            if (hovered != currentHoveredTile)
            {
                // Reset old tile
                if (currentHoveredTile != null)
                    currentHoveredTile.ResetHighlight();

                // Highlight new tile
                hovered.Highlight();
                currentHoveredTile = hovered;
            }
        }
        else
        {
            if (currentHoveredTile != null)
            {
                currentHoveredTile.ResetHighlight();
                currentHoveredTile = null;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isOutsideGrid = GridBounds.Instance != null && !GridBounds.Instance.IsInsideBounds(transform.position);

        if (isFirstDrop)
        {
            if (isOutsideGrid || currentHoveredTile == null)
            {
                Destroy(gameObject); // Despawn if first drop is invalid
                return;
            }

            // Replace existing component if necessary
            if (currentHoveredTile.currentComponent != null)
            {
                Destroy(currentHoveredTile.currentComponent);
            }

            // Snap to tile
            transform.position = currentHoveredTile.GetWorldPosition();
            originalPosition = transform.position;
            currentHoveredTile.currentComponent = gameObject;

            currentHoveredTile.ResetHighlight();
            currentHoveredTile = null;
            isFirstDrop = false;
        }
        else
        {
            if (isOutsideGrid)
            {
                Destroy(gameObject); // Despawn if moved outside grid later
                return;
            }

            if (currentHoveredTile != null)
            {
                if (currentHoveredTile.currentComponent != null && currentHoveredTile.currentComponent != gameObject)
                {
                    Destroy(currentHoveredTile.currentComponent);
                }

                transform.position = currentHoveredTile.GetWorldPosition();
                originalPosition = transform.position;
                currentHoveredTile.currentComponent = gameObject;
            }
            else
            {
                transform.position = originalPosition; // Return to last valid tile
            }

            if (currentHoveredTile != null)
            {
                currentHoveredTile.ResetHighlight();
                currentHoveredTile = null;
            }
        }
    }

    Tile FindClosestTile()
    {
        Tile[] allTiles = GameObject.FindObjectsByType<Tile>(FindObjectsSortMode.None);
        float minDistance = Mathf.Infinity;
        Tile closest = null;

        foreach (Tile tile in allTiles)
        {
            float dist = Vector2.Distance(transform.position, tile.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = tile;
            }
        }

        return closest;
    }
}
