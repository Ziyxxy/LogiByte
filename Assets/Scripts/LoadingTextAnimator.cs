using TMPro;
using UnityEngine;
using System.Collections;

public class PercentageLoader : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public float startDelay = 2f; // delay in seconds before starting
    public float duration = 3f;   // total time from 0% to 100%

    void Start()
    {
        StartCoroutine(AnimatePercentage());
    }

    IEnumerator AnimatePercentage()
    {
        // Wait before starting the percentage
        yield return new WaitForSeconds(startDelay);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float progress = Mathf.Clamp01(elapsed / duration);
            int percent = Mathf.RoundToInt(progress * 100f);
            loadingText.text = percent + "%";
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Make sure it ends exactly on 100%
        loadingText.text = "100%";
    }
}
