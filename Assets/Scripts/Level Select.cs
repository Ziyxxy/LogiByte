using UnityEngine;
using UnityEngine.UI;

public class LevelSelectToggler : MonoBehaviour
{
    public GameObject levelSelectPanel;  // The parent panel containing all buttons + background
    public Button playButton;
    public Button backButton;

    void Start()
    {
        levelSelectPanel.SetActive(false); // Ensure it's hidden at start
        playButton.onClick.AddListener(ShowLevelSelect);
        backButton.onClick.AddListener(HideLevelSelect);
    }

    void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
    }

    void HideLevelSelect()
    {
    levelSelectPanel.SetActive(false);
    }

}
