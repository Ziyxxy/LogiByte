using UnityEngine;

public class XorGate : ThreeInputLogic
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
        bool signal2 = input2 != null && input2.GetOutput();
        bool signal3 = input3 != null && input3.GetOutput();

        // Checks if And should behave with 2 or 3 inputs
        if (input1 != null && input2 != null && input3 == null) // input 3 off
        {
            state = signal1 ^ signal2;
        }
        if (input1 != null && input2 == null && input3 != null) // input 2 off
        {
            state = signal1 ^ signal3;
        }
        if (input1 == null && input2 != null && input3 != null) // input 1 off
        {
            state = signal2 ^ signal3;
        }
        if (input1 != null && input2 != null && input3 != null) // all inputs on (3 input)
        {
            state = signal1 ^ signal2 ^ signal3;
        }
        // Forward signal to connected component
        connectedOutput?.ReceiveSignal(state);
    }
}

//make sure all gate logics are being called in WireDrawer.cs