using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireBehavior : MonoBehaviour
{
    public ComponentBase startComponent;
    public ComponentBase endComponent;

    private LineRenderer lineRenderer;
    private float animationDuration = 1f;
    private float animationTimer = 0f;
    private bool isSignalOn = false;
    private Color32 StartColor = new Color32(111, 144, 137, 255);
    private Color32 EndColor = new Color32(9, 255, 203, 255);

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        CheckConnection();
    }

    void Update()
    {
        if (startComponent == null || endComponent == null) return;

        bool currentSignal = startComponent.GetOutput();
        if (currentSignal != isSignalOn)
        {
            isSignalOn = currentSignal;
            animationTimer = 0f;
        }

        if (isSignalOn)
        {
            AnimateWireColor();
        }
        else
        {
            lineRenderer.startColor = lineRenderer.endColor = StartColor;
        }
    }

    private void AnimateWireColor()
    {
        animationTimer += Time.deltaTime / animationDuration;
        float t = Mathf.Clamp01(animationTimer);
        Color color = Color.Lerp(StartColor, EndColor, t);
        lineRenderer.startColor = lineRenderer.endColor = color;
    }

    public void CheckConnection()
    {
        if (startComponent == null || endComponent == null)
        {
            Debug.LogWarning("Wire must connect two components.");
            return;
        }

        // Register wire with start component
        startComponent.AddConnectedWire(this);

        // Send initial signal
        endComponent.ReceiveSignal(startComponent.GetOutput());

        // If the endComponent accepts dynamic input linking, connect automatically
        TryAutoConnectInput(endComponent, startComponent);

        // GATE RECOGNITION SYSTEM
        if (startComponent is NotGate notGate && !(endComponent is InputNode))
            {
                notGate.ConnectOutput(endComponent);  // NOT GATE
            }
        if (startComponent is AndGate andGate && !(endComponent is InputNode))
            {
                andGate.ConnectOutput(endComponent);  // AND GATE
            }
        if (startComponent is OrGate orGate && !(endComponent is InputNode))
            {
                orGate.ConnectOutput(endComponent);  // OR GATE
            }
        if (startComponent is XorGate xorGate && !(endComponent is InputNode))
            {
                xorGate.ConnectOutput(endComponent);  // XOR GATE
            }
        if (startComponent is NandGate nandGate && !(endComponent is InputNode))
            {
                nandGate.ConnectOutput(endComponent);  // NAND GATE
            }
        if (startComponent is NorGate norGate && !(endComponent is InputNode))
            {
                norGate.ConnectOutput(endComponent);  // NOR GATE
            }
        if (startComponent is XnorGate xnorGate && !(endComponent is InputNode))
            {
                xnorGate.ConnectOutput(endComponent);  // XNOR GATE
            }
        if (startComponent is OutputNode outputGate && !(endComponent is InputNode))
            {
                outputGate.ConnectInput(endComponent);  // OUTPUT
            }
    }

    private void TryAutoConnectInput(ComponentBase target, ComponentBase inputSource)
    {
        // Look for common input slots like input1/input2 using reflection
        var type = target.GetType();
        var input1 = type.GetField("input1");
        var input2 = type.GetField("input2");
        var input3 = type.GetField("input3");

        if (input1 != null && input1.GetValue(target) == null)
        {
            input1.SetValue(target, inputSource);
        }
        else if (input2 != null && input2.GetValue(target) == null)
        {
            input2.SetValue(target, inputSource);
        }
        else if (input3 != null && input3.GetValue(target) == null)
        {
            input3.SetValue(target, inputSource);
        }

        // For OutputNodes or gates, recalculate signal
        target.ReceiveSignal(inputSource.GetOutput());
    }

    public void UpdateSignal()
    {
        if (startComponent == null || endComponent == null) return;

        bool signal = startComponent.GetOutput();
        endComponent.ReceiveSignal(signal);
    }

    public void Disconnect()
    {
        if (startComponent != null && endComponent != null)
        {
            // GATE DISCONNECT SYSTEM
            // Disconnect from OutputNode
            if (endComponent is OutputNode outputNode)
            {
                if (outputNode.input1 == startComponent)
                    outputNode.input1 = null;

                // Reset output color when disconnecting
                outputNode.ResetOutputColor();
            }

            // NOT GATE
            if (endComponent is NotGate notGate)
            {
                if (notGate.input1 == startComponent)
                    notGate.input1 = null;
            }
            if (startComponent is NotGate outputNot)
            {
                if (outputNot.connectedOutput == endComponent)
                    outputNot.connectedOutput = null;
            }

            // AND GATE
            if (endComponent is AndGate andGate)
            {
                if (andGate.input1 == startComponent)
                    andGate.input1 = null;
                else if (andGate.input2 == startComponent)
                    andGate.input2 = null;
                else if (andGate.input3 == startComponent)
                    andGate.input3 = null;
            }
            if (startComponent is AndGate outputAnd)
            {
                if (outputAnd.connectedOutput == endComponent)
                    outputAnd.connectedOutput = null;
            }

            // OR GATE
            if (endComponent is OrGate orGate)
            {
                if (orGate.input1 == startComponent)
                    orGate.input1 = null;
                else if (orGate.input2 == startComponent)
                    orGate.input2 = null;
                else if (orGate.input3 == startComponent)
                    orGate.input3 = null;
            }
            if (startComponent is OrGate outputOr)
            {
                if (outputOr.connectedOutput == endComponent)
                    outputOr.connectedOutput = null;
            }

            // XOR GATE
            if (endComponent is XorGate xorGate)
            {
                if (xorGate.input1 == startComponent)
                    xorGate.input1 = null;
                else if (xorGate.input2 == startComponent)
                    xorGate.input2 = null;
                else if (xorGate.input3 == startComponent)
                    xorGate.input3 = null;
            }
            if (startComponent is XorGate outputXor)
            {
                if (outputXor.connectedOutput == endComponent)
                    outputXor.connectedOutput = null;
            }
            
            // NAND GATE
            if (endComponent is NandGate nandGate)
            {
                if (nandGate.input1 == startComponent)
                    nandGate.input1 = null;
                else if (nandGate.input2 == startComponent)
                    nandGate.input2 = null;
                else if (nandGate.input3 == startComponent)
                    nandGate.input3 = null;
            }
            if (startComponent is NandGate outputNand)
            {
                if (outputNand.connectedOutput == endComponent)
                    outputNand.connectedOutput = null;
            }

            // NOR GATE
            if (endComponent is NorGate norGate)
            {
                if (norGate.input1 == startComponent)
                    norGate.input1 = null;
                else if (norGate.input2 == startComponent)
                    norGate.input2 = null;
                else if (norGate.input3 == startComponent)
                    norGate.input3 = null;
            }
            if (startComponent is NorGate outputNor)
            {
                if (outputNor.connectedOutput == endComponent)
                    outputNor.connectedOutput = null;
            }

            // XNOR GATE
            if (endComponent is XnorGate xnorGate)
            {
                if (xnorGate.input1 == startComponent)
                    xnorGate.input1 = null;
                else if (xnorGate.input2 == startComponent)
                    xnorGate.input2 = null;
                else if (xnorGate.input3 == startComponent)
                    xnorGate.input3 = null;
            }
            if (startComponent is XnorGate outputXnor)
            {
                if (outputXnor.connectedOutput == endComponent)
                    outputXnor.connectedOutput = null;
            }

            // Remove from input's wire list
            if (startComponent is InputNode input)
            {
                input.RemoveConnectedWire(this);
            }
        }
    }
}
