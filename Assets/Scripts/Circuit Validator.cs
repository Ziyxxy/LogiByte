using UnityEngine;
using System.Collections.Generic;

public class CircuitValidator : MonoBehaviour
{
    public InputNode input1;
    public InputNode input2;
    public OutputNode output;

    private List<(bool, bool, bool)> targetTruthTable = new List<(bool, bool, bool)>
    {
        (false, false, false),
        (false, true,  false),
        (true,  false, false),
        (true,  true,  true),
    };

    public void ValidateCircuit()
    {
        bool success = true;

        foreach (var (in1, in2, expected) in targetTruthTable)
        {
            // Set inputs
            input1.SetState(in1);
            input2.SetState(in2);

            // Force recalculation
            input1.NotifyConnectedGates();
            input2.NotifyConnectedGates();

            output.UpdateOutputFromInputs();  // if needed

            // Small delay can help in extreme cases:
            // yield return new WaitForSeconds(0.1f);

            // Read output
            bool actual = output.GetOutput();

            if (actual != expected)
            {
                Debug.LogError($"Mismatch! Inputs ({in1},{in2}) expected {expected} but got {actual}");
                success = false;
            }
        }

        if (success)
        {
            Debug.Log("✅ Circuit is correct!");
        }
        else
        {
            Debug.Log("❌ Circuit is incorrect.");
        }
    }
}
