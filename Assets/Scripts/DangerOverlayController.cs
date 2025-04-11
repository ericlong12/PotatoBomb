using UnityEngine;
using UnityEngine.UI;

public class DangerOverlayController : MonoBehaviour
{
    public Image overlayImage; // Drag your DangerOverlay Image here
    public Potato potato;      // Drag your Potato GameObject here

    void Update()
    {
        if (potato == null || overlayImage == null)
            return;

        if (potato.maxCountdown == 0)
            return; // Avoid division by zero

        // 1. Calculate danger level: 0 (safe) ➔ 1 (about to explode)
        float dangerLevel = 1f - (potato.countdown / potato.maxCountdown);

        // 2. Lerp the alpha of the red overlay
        Color color = overlayImage.color;
        color.a = Mathf.Lerp(0f, 0.7f, dangerLevel); // Max alpha 0.7 (not 100% red)
        overlayImage.color = color;
    }
}
