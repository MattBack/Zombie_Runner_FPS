using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    EnemyHealth enemy;
    PlayerHealth target;
    // delay before grenade explodes
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;

    bool hasExploded = false;

    public GameObject explosionEffect;

    [SerializeField]
    private AudioClip explosionSound;
    private AudioSource audioSource;


    // countdown 
    float countdown;

    // private TraumaInducer trauma;


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No sound bro");
        }
        else
        {
            audioSource.clip = explosionSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        //trauma.Delay = countdown;
        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            //trauma = target.GetComponent<TraumaInducer>();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // show effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        // play sound
        audioSource.Play();

        // get all nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            // add force and damage to enemy - maybe random range?
            EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // if target has enemy health script, they take damage;
                // TODO: could check for generic or object damage script in future?
                enemyHealth.TakeDamage(50);
            }
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
                // call trauma inducer on player
            }

        }

        // remove grenade
        Destroy(gameObject, 3);

        // Destroy explosion effect after a delay
        float explosionDuration = explosion.GetComponent<ParticleSystem>().main.duration;
        Destroy(explosion, explosionDuration);
    }
}
