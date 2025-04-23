using UnityEngine;
using UnityEngine.EventSystems;

public class GateSpawner : MonoBehaviour
{
    public GameObject gatePrefab;
    private GameObject spawnedGate;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        if (gatePrefab == null) return;

        Vector3 spawnPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0f;

        spawnedGate = Instantiate(gatePrefab, spawnPosition, Quaternion.identity);

        var draggable = spawnedGate.GetComponent<Draggable>();
        if (draggable != null)
            draggable.enabled = true;

        // Simulate dragging
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        ExecuteEvents.Execute(spawnedGate, pointerData, ExecuteEvents.beginDragHandler);
        ExecuteEvents.Execute(spawnedGate, pointerData, ExecuteEvents.dragHandler);
    }

    void Update()
    {
        if (spawnedGate != null && Input.GetMouseButton(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            ExecuteEvents.Execute(spawnedGate, pointerData, ExecuteEvents.dragHandler);
        }

        if (spawnedGate != null && Input.GetMouseButtonUp(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            ExecuteEvents.Execute(spawnedGate, pointerData, ExecuteEvents.endDragHandler);
            spawnedGate = null;
        }
    }
}
