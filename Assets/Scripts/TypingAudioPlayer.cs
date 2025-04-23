using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingAudioPlayer : MonoBehaviour
{
    public AudioSource typingSound;   // The AudioSource for the typing sound
    private TextMeshProUGUI tmpText;
    private string previousText = "";
    private bool toggle = true;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if ((tmpText.text != previousText) & (toggle == true))
        {
            typingSound.Play();
            toggle = false;
        }
    }
}
