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

        if (startComponent is NotGate notGate && !(endComponent is InputNode))
            {
                notGate.ConnectOutput(endComponent);  // Assign output
            }
        if (startComponent is AndGate andGate && !(endComponent is InputNode))
            {
                andGate.ConnectOutput(endComponent);  // Assign output
            }
        if (startComponent is OrGate orGate && !(endComponent is InputNode))
            {
                orGate.ConnectOutput(endComponent);  // Assign output
            }
        if (startComponent is OutputNode outputGate && !(endComponent is InputNode))
            {
                outputGate.ConnectInput(endComponent);  // Assign output
            }
    }

    private void TryAutoConnectInput(ComponentBase target, ComponentBase inputSource)
    {
        // Look for common input slots like input1/input2 using reflection
        var type = target.GetType();
        var input1 = type.GetField("input1");
        var input2 = type.GetField("input2");

        if (input1 != null && input1.GetValue(target) == null)
        {
            input1.SetValue(target, inputSource);
        }
        else if (input2 != null && input2.GetValue(target) == null)
        {
            input2.SetValue(target, inputSource);
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
            // Disconnect from OutputNode
            if (endComponent is OutputNode outputNode)
            {
                if (outputNode.input1 == startComponent)
                    outputNode.input1 = null;

                // Reset output color when disconnecting
                outputNode.ResetOutputColor();
            }

            // Disconnect from AndGate
            if (endComponent is AndGate andGate)
            {
                if (andGate.input1 == startComponent)
                    andGate.input1 = null;
                else if (andGate.input2 == startComponent)
                    andGate.input2 = null;
            }
            // Disconnect gate output AND
            if (startComponent is AndGate outputAnd)
            {
                if (outputAnd.connectedOutput == endComponent)
                    outputAnd.connectedOutput = null;
            }

            // Disconnect from OrGate
            if (endComponent is OrGate orGate)
            {
                if (orGate.input1 == startComponent)
                    orGate.input1 = null;
                else if (orGate.input2 == startComponent)
                    orGate.input2 = null;
            }
            // Disconnect gate output OR
            if (startComponent is OrGate outputOr)
            {
                if (outputOr.connectedOutput == endComponent)
                    outputOr.connectedOutput = null;
            }

            // Remove from input's wire list
            if (startComponent is InputNode input)
            {
                input.RemoveConnectedWire(this);
            }
        }
    }
}
