using UnityEngine;

public class ComponentBase : MonoBehaviour
{
    public virtual void AddConnectedWire(WireBehavior wire) 
    {
        // Default: do nothing. Override in InputNode or others if needed.
    }

    public virtual bool GetOutput()
    {
        return false;
    }

    public virtual void ReceiveSignal(bool signal)
    {
    }
}
