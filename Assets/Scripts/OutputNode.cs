using UnityEngine;

public class OutputNode : ComponentBase
{
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
        GetComponent<SpriteRenderer>().color = state ? Color.green : Color.red;
    }

    public void ResetOutputColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
