using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    PlayerHealth target;
    [SerializeField] float damage = 40f;
    //[SerializeField] float destroyTimer = 8f;


    void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;
        target.TakeDamage(damage);
        target.GetComponent<DisplayDamage>().ShowDamageImpact();
        target.GetComponent<TraumaInducer>();
        Invoke("Register", 0f);
    }

    void Register()
    {
        if (!DI_System.CheckIfObjectInSight(this.transform))
        {
            DI_System.CreateIndicator(this.transform);
        }
        //Destroy(this.gameObject, destroyTimer);

    }
    
}
