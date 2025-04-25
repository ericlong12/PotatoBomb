using UnityEngine;
using TMPro;

public class PlayerLegend : MonoBehaviour
{
    public GameObject legendPanel; // Assign this in Inspector

    [Header("Player Legend Texts")]
    public TMP_Text[] playerTexts = new TMP_Text[6]; // Assign Player1Text, Player2Text, etc.

    void Start()
    {
        legendPanel.SetActive(false);

        // Set legend text based on keybinds
        for (int i = 0; i < playerTexts.Length; i++)
        {
            string savedKey = PlayerPrefs.GetString("Player" + i + "_Key", "None");
            playerTexts[i].text = "Player " + (i + 1) + ": " + savedKey;
        }
    }

    void Update()
    {
        // Show legend when Spacebar is held
        legendPanel.SetActive(Input.GetKey(KeyCode.Space));
    }
}
