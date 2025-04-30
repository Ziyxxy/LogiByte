using UnityEngine;
using UnityEngine.UI;

public class LevelSelectToggler : MonoBehaviour
{
    public GameObject levelSelectPanel;  // The parent panel containing all buttons + background
    public GameObject settingPanel;
    public Button playButton;
    public Button backButton;
    public Button settingButton;
    public Button settingBackButton;
    
    void Start()
    {
        levelSelectPanel.SetActive(false); // Ensure it's hidden at start
        settingPanel.SetActive(false);
        playButton.onClick.AddListener(ShowLevelSelect);
        backButton.onClick.AddListener(HideLevelSelect);
        settingButton.onClick.AddListener(ShowSettingMenu);
        settingBackButton.onClick.AddListener(HideSettingMenu);
    }

    void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
    }

    void HideLevelSelect()
    {
        levelSelectPanel.SetActive(false);
    }

    void ShowSettingMenu()
    {
        settingPanel.SetActive(true);
    }

    void HideSettingMenu()
    {
        settingPanel.SetActive(false);
    }

}
