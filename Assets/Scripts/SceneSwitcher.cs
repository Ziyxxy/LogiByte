using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSwitcher : MonoBehaviour
{
    public float waitTimeBeforeSwitchAllowed = 6f;
    public TextMeshProUGUI loadingText; // Optional: to update the UI when ready

    private bool canSwitch = false;

    void Start()
    {
        Invoke(nameof(EnableSceneSwitch), waitTimeBeforeSwitchAllowed);
    }

    void EnableSceneSwitch()
    {
        canSwitch = true;
        if (loadingText != null)
        {
            loadingText.text += " (Press SPACE to Continue...)";
        }
    }

    void Update()
    {
        if (canSwitch && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
