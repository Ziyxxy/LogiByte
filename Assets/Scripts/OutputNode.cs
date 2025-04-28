using UnityEngine;

public class OutputNode : ComponentBase
{
    private Color32 StartColor = new Color32(111, 144, 137, 255);
    private Color32 EndColor = new Color32(9, 255, 203, 255);
    private bool state = false;

    // Only one input allowed
    public ComponentBase input1;

    public override bool GetOutput()
    {
        return state;
    }

    public override void ReceiveSignal(bool _)
    {
        UpdateOutputFromInputs();  // Recalculate based on input
    }

    public void ConnectInput(ComponentBase input)
    {
        input1 = input;  // Only allows one input to connect
        UpdateOutputFromInputs();
    }

    public void UpdateOutputFromInputs()
    {
        bool signal1 = input1 != null && input1.GetOutput();

        state = signal1;  // Directly take input's state as output (no OR/AND logic anymore)
        UpdateOutputColor();
    }

    private void UpdateOutputColor()
    {
        GetComponent<SpriteRenderer>().color = state ? EndColor : StartColor;
    }

    public void ResetOutputColor()
    {
        GetComponent<SpriteRenderer>().color = StartColor;
    }
}
