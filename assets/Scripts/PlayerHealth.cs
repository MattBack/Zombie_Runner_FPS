using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider slider;

    public HealthBar healthBar;

    public float maxHitPoints = 100f;
    public float hitPoints;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        hitPoints = maxHitPoints;
        healthBar.SetMaxHealth(maxHitPoints);
    }

    public void IncreaseHealth(float healthAmount)
    {
        hitPoints += healthAmount;
    }

    public void TakeDamage(float damage) {
        hitPoints -= damage;
        audioManager.Play("PlayerTakeDamage");
        healthBar.SetHealth(hitPoints);
        if (hitPoints < 50)
        {
            audioManager.Play("LowHealth");
        }
        else {
            return;
        }


        if (hitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
            audioManager.Play("PlayerDeath");
            audioManager.Play("DeathImpact");
        }
    }
}
