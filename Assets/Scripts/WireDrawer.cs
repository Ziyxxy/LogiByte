using UnityEngine;
using System.Collections.Generic;

public class WireDrawer : MonoBehaviour
{
    public GameObject wirePrefab;
    private LineRenderer currentLine;
    private bool isDrawing = false;
    private Camera cam;
    private List<GameObject> wireHistory = new List<GameObject>();
    private List<Vector3> drawnPoints = new List<Vector3>();
    

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z))
        {
            if (Application.isPlaying)
                UndoLastWire();
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetMouseButtonDown(0))
                StartDrawing();
            else if (Input.GetMouseButton(0) && isDrawing)
                UpdateDrawing();
            else if (Input.GetMouseButtonUp(0) && isDrawing)
                FinishDrawing();
        }
    }

    Vector3 GetSnappedMousePosition()
    {
        Vector3 m = cam.ScreenToWorldPoint(Input.mousePosition);
        m.z = 0;

        Tile[] tiles = GameObject.FindObjectsByType<Tile>(FindObjectsSortMode.None);
        float best = Mathf.Infinity;
        Tile closest = null;
        foreach (var t in tiles)
        {
            float d = Vector2.Distance(t.transform.position, m);
            if (d < best)
            {
                best = d;
                closest = t;
            }
        }
        return (closest != null) ? closest.GetWorldPosition() : m;
    }

    void StartDrawing()
    {
        GameObject wire = Instantiate(wirePrefab);
        currentLine = wire.GetComponent<LineRenderer>();
        currentLine.useWorldSpace = true;
        drawnPoints.Clear();

        Vector3 start = GetSnappedMousePosition();
        drawnPoints.Add(start);
        currentLine.positionCount = 1;
        currentLine.SetPosition(0, start);

        wireHistory.Add(wire);
        isDrawing = true;
    }

    void UpdateDrawing()
    {
        if (currentLine == null) return;

        Vector3 currentMouseTile = GetSnappedMousePosition();
        Vector3 lastPoint = drawnPoints[drawnPoints.Count - 1];

        // Only allow axis-aligned segments (horizontal or vertical)
        bool movedHorizontally = Mathf.Abs(currentMouseTile.x - lastPoint.x) > 0.1f && Mathf.Abs(currentMouseTile.y - lastPoint.y) < 0.1f;
        bool movedVertically = Mathf.Abs(currentMouseTile.y - lastPoint.y) > 0.1f && Mathf.Abs(currentMouseTile.x - lastPoint.x) < 0.1f;

        if ((movedHorizontally || movedVertically) && !drawnPoints.Contains(currentMouseTile))
        {
            drawnPoints.Add(currentMouseTile);
            currentLine.positionCount = drawnPoints.Count;
            currentLine.SetPosition(drawnPoints.Count - 1, currentMouseTile);
        }
    }

    void FinishDrawing()
    {
        isDrawing = false;

        if (currentLine != null)
        {
            Vector3 start = currentLine.GetPosition(0);
            Vector3 end = currentLine.GetPosition(currentLine.positionCount - 1);

            bool startValid = IsNearAnyTile(start);
            bool endValid = IsNearAnyTile(end);

            if (!startValid || !endValid || Vector3.Distance(start, end) < 0.1f)
            {
                Destroy(currentLine.gameObject); // Invalid wire
            }
            else
            {
                // Try to get components at both ends
                ComponentBase startComp = GetComponentAtPosition(start);
                ComponentBase endComp = GetComponentAtPosition(end);

                // ✅ FIXED: Declare 'behavior'
                WireBehavior behavior = currentLine.GetComponent<WireBehavior>();
                if (behavior != null)
                    {
                        behavior.startComponent = startComp;
                        behavior.endComponent = endComp;

                        behavior.CheckConnection(); // optional
                    }
            }
        }

        currentLine = null;
    }

    bool IsNearAnyTile(Vector3 point)
    {
        Tile[] tiles = GameObject.FindObjectsByType<Tile>(FindObjectsSortMode.None);
        foreach (Tile tile in tiles)
        {
            if (Vector3.Distance(tile.transform.position, point) < 0.4f)
                return true;
        }
        return false;
    }

    void UndoLastWire()
    {
        if (wireHistory.Count > 0)
        {
            GameObject lastWire = wireHistory[^1];
            wireHistory.RemoveAt(wireHistory.Count - 1);

            WireBehavior wireBehavior = lastWire.GetComponent<WireBehavior>();
            if (wireBehavior != null)
            {
                wireBehavior.Disconnect(); // Clear all connections
            }

            Destroy(lastWire);
        }
    }

    ComponentBase GetComponentAtPosition(Vector3 position)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(position);
        foreach (var hit in hits)
        {
            var comp = hit.GetComponent<ComponentBase>();
            if (comp != null)
                return comp;
        }
        return null;
    }
}
