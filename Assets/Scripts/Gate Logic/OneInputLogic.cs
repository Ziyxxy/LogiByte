using UnityEngine;

public abstract class OneInputLogic : ComponentBase
{
    public ComponentBase input1;
    public ComponentBase connectedOutput;

    public void ConnectInput1(ComponentBase input)
    {
        input1 = input;
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
    }

    public abstract void UpdateOutputFromInputs();
}
