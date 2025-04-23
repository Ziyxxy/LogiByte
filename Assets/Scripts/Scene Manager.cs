using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Load the Play scene

    public void LoadLevelOneScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevelTwoScene()
    {
        SceneManager.LoadScene("Level2");
    }

        public void LoadLevelThreeScene()
    {
        SceneManager.LoadScene("Level3");
    }

    public void LoadLevelFourScene()
    {
        SceneManager.LoadScene("Level4");
    }

    // Load the Settings scene
    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

    // Quit the application
    public void ExitGame()
    {
        Application.Quit();

        // Optional: This helps show the quit works in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
