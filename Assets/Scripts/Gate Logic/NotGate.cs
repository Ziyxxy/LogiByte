using UnityEngine;

public class NotGate : OneInputLogic
{
    private bool state = false;

    public override bool GetOutput()
    {
        return state;
    }

    public override void ReceiveSignal(bool _)
    {
        // Recompute on any signal change
        UpdateOutputFromInputs();
    }

    public override void UpdateOutputFromInputs()
    {
        bool signal1 = input1 != null && input1.GetOutput();

        state = !signal1;

        // Forward signal to connected component
        connectedOutput?.ReceiveSignal(state);
    }
}
