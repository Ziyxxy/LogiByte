using TMPro;
using UnityEngine;

public class ObjectiveTransition : MonoBehaviour
{
    public GameObject firstConsoleText;
    public GameObject secondConsoleText;
    public GameObject thirdConsoleText;

    public GameObject firstValidator;
    public GameObject secondValidator;
    public GameObject thirdValidator;

    public GameObject secondNext;
    public GameObject thirdNext;

    public void SecondPart()
    {
        firstConsoleText.SetActive(false);
        secondNext.SetActive(false);
        firstValidator.SetActive(false);

        secondConsoleText.SetActive(true);
        secondValidator.SetActive(true);
    }

    public void ThirdPart()
    {
        secondConsoleText.SetActive(false);
        thirdNext.SetActive(false);
        secondValidator.SetActive(false);

        thirdConsoleText.SetActive(true);
        thirdValidator.SetActive(true);
    }
}
