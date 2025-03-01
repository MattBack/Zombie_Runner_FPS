using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float hitPoints)
    {
        slider.maxValue = hitPoints;
        slider.value = hitPoints;
    }

    public void SetHealth(float hitPoints)
    {
        slider.value = hitPoints;
    }

    public void IncreaseHealthSlider(float healthAmount)
    {
        slider.value += healthAmount;
    }


}
