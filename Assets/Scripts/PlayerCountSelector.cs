using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class PlayerCountSelector : MonoBehaviour
{
    [SerializeField] private Slider playerCountSlider;
    [SerializeField] private TMP_Text playerCountText; 

    private void Start()
    {
        // Initialize display
        UpdatePlayerCountDisplay(playerCountSlider.value);

        // Listen for slider changes
        playerCountSlider.onValueChanged.AddListener(UpdatePlayerCountDisplay);
    }

    public void UpdatePlayerCountDisplay(float value)
    {
        int playerCount = Mathf.RoundToInt(value);
        playerCountText.text = playerCount + " Players";

        // Store player count in a global manager, player prefs, or directly into a GameManager
        PlayerPrefs.SetInt("PlayerCount", playerCount);  
    }
}
