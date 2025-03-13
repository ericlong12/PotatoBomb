using UnityEngine;

public class PlayerLegend : MonoBehaviour
{
    public GameObject legendPanel; // Assign in Inspector

    void Start()
    {
        // Hide legend at start
        legendPanel.SetActive(false);
    }

    void Update()
    {
        // Show legend when Spacebar is held, hide when released
        if (Input.GetKey(KeyCode.Space))
        {
            legendPanel.SetActive(true);
        }
        else
        {
            legendPanel.SetActive(false);
        }
    }
}
