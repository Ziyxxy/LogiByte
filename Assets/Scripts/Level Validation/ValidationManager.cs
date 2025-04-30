using UnityEngine;

public class ValidationManager : MonoBehaviour
{
    public AndValidator validator1;
    public OrValidator validator2;
    public NorValidator validator3;

    public void RunActiveValidator()
    {
        if (validator1 != null && validator1.gameObject.activeInHierarchy)
        {
            validator1.StartValidation();
        }
        else if (validator2 != null && validator2.gameObject.activeInHierarchy)
        {
            validator2.StartValidation();
        }
        else if (validator3 != null && validator3.gameObject.activeInHierarchy)
        {
            validator3.StartValidation();
        }
        else
        {
            Debug.LogWarning("⚠ No active validator found.");
        }
    }
}
