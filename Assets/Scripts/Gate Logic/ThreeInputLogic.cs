using UnityEngine;

public abstract class ThreeInputLogic : ComponentBase
{
    public ComponentBase input1;
    public ComponentBase input2;
    public ComponentBase input3;
    public ComponentBase connectedOutput;

    public void ConnectInput1(ComponentBase input)
    {
        input1 = input;
        UpdateOutputFromInputs();
    }

    public void ConnectInput2(ComponentBase input)
    {
        input2 = input;
        UpdateOutputFromInputs();
    }

    public void ConnectInput3(ComponentBase input)
    {
        input3 = input;
        UpdateOutputFromInputs();
    }

    public void ConnectOutput(ComponentBase output)
    {
        connectedOutput = output;
        UpdateOutputFromInputs(); // Send output immediately
    }

    public virtual void DisconnectOutput(ComponentBase target)
    {
        if (connectedOutput == target) connectedOutput = null;
    }

    public virtual void DisconnectInput(ComponentBase source)
    {
        if (input1 == source) input1 = null;
        else if (input2 == source) input2 = null;
    }

    public abstract void UpdateOutputFromInputs();
}
