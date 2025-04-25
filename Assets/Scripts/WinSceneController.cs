using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI winText;
    public Image rainbowBackground;

    [Header("Winner Potato")]
    public RectTransform winnerPotato;
    public float spinSpeed = 300f;
    public float colorChangeSpeed = 2f;

    private float hue = 0f;

    void Start()
    {
        // Ensure the game isn't paused
        Time.timeScale = 1f;

        // Get winner name from PlayerPrefs
        string winnerName = PlayerPrefs.GetString("WinnerName", "Someone");
        winText.text = winnerName + " Won!!";

        // Optional: scale up potato in case it's not big
        if (winnerPotato != null)
        {
            winnerPotato.transform.localScale = Vector3.one * 2f;
        }
    }

    void Update()
    {
        // 🌈 Rainbow background color cycling
        if (rainbowBackground != null)
        {
            hue += Time.deltaTime * colorChangeSpeed;
            if (hue > 1f) hue -= 1f;

            rainbowBackground.color = Color.HSVToRGB(hue, 1f, 1f);
        }

        // 🌀 Spin the potato!
        if (winnerPotato != null)
        {
            winnerPotato.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
        }
    }
}
