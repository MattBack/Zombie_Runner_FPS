using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float healthAmount = 10;

    PlayerHealth playerHealth;
    [SerializeField] GameObject healthEffect;

    //private void Start()
    //{
    //    //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    //    if (CompareTag("Player"))
    //    {
    //        playerHealth = GetComponent<PlayerHealth>();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (other.gameObject.tag == "Player" && playerHealth.hitPoints < playerHealth.maxHitPoints)
        {
            FindObjectOfType<PlayerHealth>().IncreaseHealth(healthAmount);
            FindObjectOfType<HealthBar>().IncreaseHealthSlider(healthAmount);
            FindObjectOfType<AudioManager>().Play("HealthPickupSfx");

            if (healthEffect != null)
            {
                GameObject effectInstance = Instantiate(healthEffect, playerHealth.transform.position, Quaternion.identity);
                //healthEffect.SetActive(true);
                effectInstance.SetActive(true);

                effectInstance.transform.SetParent(playerHealth.transform);
                
                healthEffect.transform.parent = null;
            }

            Destroy(gameObject);
        }
    }
}
