using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private float damage;

    public void meleeAttack(Animator weaponAnimator, float meleeAttackDamage) {
        damage = meleeAttackDamage;
        Debug.Log("Damage amount: " + damage);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider entered: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Found Enemy Tag");
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                Debug.Log("TAKE DAMAGE!");
                enemyHealth.TakeDamage(damage);  // Apply damage to the enemy
            }
        }
    }

}
