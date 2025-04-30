using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrValidator : MonoBehaviour
{
    public InputNode input1;
    public InputNode input2;
    public OutputNode output;

    public GameObject thirdNext;

    public float delayBetweenSteps = 0.1f;  // Customize in inspector

    private List<(bool, bool, bool)> targetTruthTable = new List<(bool, bool, bool)>
    {
        (false, false, false),
        (false, true,  true),
        (true,  false, true),
        (true,  true,  true),
    };

    void OnEnable()
    {
        HideSecondNext();
        StartCoroutine(InitAfterDelay());
    }

    IEnumerator InitAfterDelay()
    {
        yield return null;  // wait one frame

        var inputs = Object.FindObjectsByType<InputNode>(FindObjectsSortMode.None);
        output = Object.FindFirstObjectByType<OutputNode>();

        if (inputs.Length >= 2)
        {
            input1 = inputs[0];
            input2 = inputs[1];
        }
        else
        {
            Debug.LogError("❌ Could not find two input nodes!");
        }

        if (output == null)
        {
            Debug.LogError("❌ Could not find output node!");
        }
    }

    public void StartValidation()
    {
        StartCoroutine(ValidateCircuit());
    }

    private IEnumerator ValidateCircuit()
    {
        bool success = true; // Declare once outside the loop

        for (int i = 0; i < 4; i++){
            foreach (var (in1, in2, expected) in targetTruthTable)
            {
                input1.SetState(in1);
                input2.SetState(in2);

                input1.NotifyConnectedGates();
                input2.NotifyConnectedGates();
                output.UpdateOutputFromInputs();

                yield return new WaitForSeconds(delayBetweenSteps);

                bool actual = output.GetOutput();

                if (actual != expected)
                {
                    //Debug.LogError($"❌ Mismatch: Inputs ({in1}, {in2}) → Expected: {expected}, Got: {actual}");
                    success = false;
                }
                else
                {
                    //Debug.Log($"✅ Inputs ({in1}, {in2}) → Output: {actual} ✔");
                }
            }

            if (success)
            {
                Debug.Log("✅ Circuit is fully correct!");
                ShowSecondNext();
            }
            else
            {
                Debug.Log("❌ Circuit has errors.");
            }
        }
    }

    void ShowSecondNext()
    {
        thirdNext.SetActive(true);
    }

    void HideSecondNext()
    {
        thirdNext.SetActive(false);
    }
}
