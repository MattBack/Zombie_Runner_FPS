using UnityEngine;
using TMPro;

public class TextFlicker : MonoBehaviour
{
    [Range(0f, 1f)]
    public float glowIntensity = 1;

    private TextMeshProUGUI textMeshProText;
    private Material textGlow;

    // Adjust this value to control the flicker speed
    public float flickerInterval = 0.5f; // Set to the desired interval in seconds
    private float flickerTimer;

    private void Start()
    {
        textMeshProText = GetComponent<TextMeshProUGUI>();

        if (textMeshProText != null)
        {
            textGlow = textMeshProText.font.material;
            textGlow.SetFloat("_GlowPower", 0);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Accumulate time in the timer
        flickerTimer += Time.deltaTime;

        // Check if the accumulated time exceeds the flicker interval
        if (flickerTimer >= flickerInterval)
        {
            // Reset the timer
            flickerTimer = 0f;

            // Update glow intensity
            glowIntensity = Random.Range(0f, 1f);
            UpdateGlowEffect();
        }
    }

    void UpdateGlowEffect()
    {
        if (textGlow != null)
        {
            // Now you can use textGlow in your UpdateGlowEffect method
            // For example, you might adjust the glow intensity like this:
            textGlow.SetFloat("_GlowPower", glowIntensity);
        }
        else
        {
            Debug.LogError("Text glow material not initialized properly.");
        }
    }
}
