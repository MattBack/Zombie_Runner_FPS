using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public bool playerHasWeapon = (false);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //FindObjectOfType<Ammo>().IncreaseCurrentAmmo(ammoType, ammoAmount);
            //audioManager.Play("PickupAmmo");
            Destroy(gameObject);
        }
    }
}
