using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class EnemyHealth : MonoBehaviour
{
    public EnemySpawnManager EnemySpawnManager;

    public DamageNumber numberPrefab;

    [SerializeField] float hitPoints = 100f;
    [SerializeField] GameObject ammoPickup;

    public GameObject[] dropItems;

    //AudioManager audioManager;
    public AudioClip enemyDeathAudioClip; // clip that is played at point - currently very quiet
    public AudioClip enemyHurtAudioClip; // update to use audio manager?
    GameEventsManager gameEventsManager;

    bool isDead = false;

    private void Awake()
    {
        //audioManager = FindObjectOfType<AudioManager>();
        EnemySpawnManager = FindObjectOfType<EnemySpawnManager>();
        gameEventsManager = FindObjectOfType<GameEventsManager>();
    }

    public bool IsDead()
    {
        return isDead;
    }

    // create a  public method which reduces hitPoint by amount of damage from weapon

    public void TakeDamage(float damage)
    {
        BroadcastMessage("OnDamageTaken");

        EnemyHurtSfx();

        if (numberPrefab)
        {
            DamageNumber damageNumber = numberPrefab.Spawn(transform.position, damage);
            hitPoints -= damage;
            

        }
        else
        {
            hitPoints -= damage;
        }

        if (hitPoints <= 0)
            {
                Die();
            }
    }

    void EnemyHurtSfx()
    {
        //audioManager.Play("EnemyHurt");
        //audioManager.Play("EnemyHurt");
        //audioManager.Play("EnemyBulletHit");
        if (enemyHurtAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(enemyHurtAudioClip, transform.position); // TODO refactor
        }
        else
        {
            Debug.LogWarning($"No enemyHurtAudioClip assigned on {gameObject.name}");
        }
       
    }

    void EnemyDeathSfx() {
        //audioManager.Play("EnemyDeathSfx");
        //audioManager.Play("BloodSplatLongSfx");

        if (enemyDeathAudioClip == null)
        {
            Debug.LogWarning($"No enemyDeathAudioClip assigned on {gameObject.name}");
            return;
        }
        else {
            AudioSource.PlayClipAtPoint(enemyDeathAudioClip, transform.position);
        }
        
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        EnemyDeathSfx();
        // Allow random death animation - Set up in Animator, currently 3: Die_1, Die_2, Die_3
        int randomDeathAnimation = Random.Range(1, 4);
        GetComponent<Animator>().SetBool("Die_" + randomDeathAnimation, true);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
        EnemySpawnManager.enemyCount--;
        gameEventsManager.KillCount++;
        DropAmmo();
        GameObject.Destroy(this.gameObject, 3f); //TODO: If we want zombies to dissapear after a period of time

    }

    void DropAmmo()
    {
        int randomNumber = Mathf.RoundToInt(Random.Range(0f, dropItems.Length - 1));

        Vector3 position = transform.position;
        GameObject itemDrop = Instantiate(dropItems[randomNumber], position, Quaternion.identity);
        //Destroy(ammoPickup, 3f); TODO: remove if not needed;

    }

}
