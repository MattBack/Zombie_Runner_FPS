using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The reference the slider for the health bar")]
    public Slider slider;
    public HealthBar healthBar;
    [Tooltip("Sets the max health that a player has")]
    public float maxHitPoints = 100f;
    [Tooltip("The current number of hitpoints the player has")]
    public float hitPoints;

    [Tooltip("Sets the threshold of health for events to be triggered from (e.g. glow/heartbeat sounds)")]
    public float thresholdAmount = 0.5f;
    private float hitpointsThresholdPercentage;
    
    AudioManager audioManager;

    [Header("Health Bar glowing effects")]
    private Outline outline;
    private Coroutine glowCoroutine;
    private bool isGlowing = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        hitpointsThresholdPercentage = maxHitPoints * thresholdAmount ;
        // Find the Outline component
        Transform backgroundHealthTransform = healthBar.transform.Find("BackgroundHealth");
        if (backgroundHealthTransform != null)
        {
            outline = backgroundHealthTransform.GetComponent<Outline>();
            if (outline == null)
            {
                Debug.LogError("Outline component not found on BackgroundHealth GameObject!");
            }
            else
            {
                // Start with Alpha = 0 (invisible)
                SetOutlineAlpha(0f);
            }
        }
        else
        {
            Debug.LogError("BackgroundHealth GameObject not found in HealthBar!");
        }

        hitPoints = maxHitPoints;
        healthBar.SetMaxHealth(maxHitPoints);
    }

    public void IncreaseHealth(float healthAmount)
    {
        hitPoints += healthAmount;
        healthBar.SetHealth(hitPoints);
        CheckHealthState();
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        audioManager.Play("PlayerTakeDamage");
        healthBar.SetHealth(hitPoints);
        CheckHealthState();

        if (hitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
            audioManager.Play("PlayerDeath");
            audioManager.Play("DeathImpact");
        }
    }

    private void CheckHealthState()
    {
        if (hitPoints < hitpointsThresholdPercentage)
        {
            audioManager.Play("LowHealth");

            if (!isGlowing && outline != null)
            {
                glowCoroutine = StartCoroutine(GlowEffect());
                isGlowing = true;
            }
        }
        else
        {
            if (isGlowing && outline != null)
            {
                StopCoroutine(glowCoroutine);
                ResetOutlineAlpha();
                isGlowing = false;
            }
        }
    }

    private IEnumerator GlowEffect()
    {
        float duration = 1.5f;
        float alphaMin = 0f;
        float alphaMax = 0.4f;

        while (hitPoints < hitpointsThresholdPercentage)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / duration)
            {
                float alpha = Mathf.Lerp(alphaMin, alphaMax, Mathf.Sin(t * Mathf.PI));
                SetOutlineAlpha(alpha);
                yield return null;
            }
        }

        ResetOutlineAlpha(); // Ensure alpha is reset when exiting the loop
    }

    private void SetOutlineAlpha(float alpha)
    {
        if (outline != null)
        {
            Color effectColor = outline.effectColor;
            effectColor.a = alpha;
            outline.effectColor = effectColor;
        }
    }

    private void ResetOutlineAlpha()
    {
        if (outline != null)
        {
            Color effectColor = outline.effectColor;
            effectColor.a = 0f;
            outline.effectColor = effectColor;
        }
    }
}
