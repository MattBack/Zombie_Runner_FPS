using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetection : MonoBehaviour
{
    public bool onFire;
    public bool takingFireDamage;
    public int fireDamageAmount = 5;
    public int fireDamageSpeed = 1;

    // reference to PlayerHealth Script on player
    public PlayerHealth PlayerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire == true)
        {
            if (takingFireDamage == false) {
                takingFireDamage = true;
                StartCoroutine(DamageFromFire());
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fire" || other.tag == "Fire")
        {
            onFire = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "fire" || other.tag == "Fire")
        {
            onFire = false;
        }
    }

    IEnumerator DamageFromFire()
    {

        yield return new WaitForSeconds(fireDamageSpeed);
        PlayerHealth.TakeDamage(fireDamageAmount);
        takingFireDamage = false;
    }

}
