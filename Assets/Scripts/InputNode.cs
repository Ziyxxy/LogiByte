using UnityEngine;
using System.Collections.Generic;

public class InputNode : ComponentBase
{
    public string inputName;
    private bool state;
    private SpriteRenderer spriteRenderer;
    public List<WireBehavior> connectedWires = new List<WireBehavior>();
    private OutputNode connectedOutput; // Store the connected Output node

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            return;

        state = !state;
        UpdateColor();
        
        // Notify connected wires to update signal
        for (int i = connectedWires.Count - 1; i >= 0; i--)
        {
            var wire = connectedWires[i];
            if (wire == null)
                connectedWires.RemoveAt(i);
            else
                wire.UpdateSignal();
        }
    }

    public override bool GetOutput()
    {
        return state;
    }

    public override void ReceiveSignal(bool signal)
    {
        state = signal;
        UpdateColor();
    }

    public override void AddConnectedWire(WireBehavior wire)
    {
        if (wire != null && !connectedWires.Contains(wire))
            connectedWires.Add(wire);
    }

    public void RemoveConnectedWire(WireBehavior wire)
    {
        connectedWires.Remove(wire);
    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = state ? Color.green : Color.red;
    }

    // Method to connect this InputNode to an OutputNode
    public void ConnectToOutput(OutputNode output)
    {
        if (output != null && connectedOutput != output)
        {
            connectedOutput = output;
            output.ReceiveSignal(state); // Transfer the current state
        }
    }

    public void NotifyConnectedGates()
    {
        foreach (var wire in connectedWires)
        {
            if (wire.endComponent != null)
            {
                wire.endComponent.ReceiveSignal(state);
            }
        }
    }

    public void SetState(bool value)
    {
        this.state = value;
        for (int i = connectedWires.Count - 1; i >= 0; i--)
        {
            var wire = connectedWires[i];
            if (wire == null)
                connectedWires.RemoveAt(i);
            else
                wire.UpdateSignal();
        }
    }
}
