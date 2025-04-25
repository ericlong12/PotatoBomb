using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class KeybindSetter : MonoBehaviour, IPointerClickHandler
{
    public int playerIndex; // 0 for Player 1, 1 for Player 2, etc.
    public TMP_Text keyText; // Reference to the TMP text on the button

    private bool awaitingInput = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        awaitingInput = true;
        keyText.text = "Press any key...";
    }

    void Update()
    {
        if (!awaitingInput) return;

        // Loop through all possible KeyCodes
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                // Save the key to PlayerPrefs
                PlayerPrefs.SetString("Player" + playerIndex + "_Key", key.ToString());

                // Update the UI
                keyText.text = key.ToString();

                // Done waiting
                awaitingInput = false;
                break;
            }
        }
    }

    void Start()
    {
        // On startup, load saved key if it exists
        string savedKey = PlayerPrefs.GetString("Player" + playerIndex + "_Key", "Not Set");
        keyText.text = savedKey;
    }
}
