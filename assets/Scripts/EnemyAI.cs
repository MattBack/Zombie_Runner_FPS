using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    [SerializeField] AudioSource zombieSfxSource;
    //public AudioClip zombieSfxClip;
    //[SerializeField] AudioClip zombieTakeDamage;
    [SerializeField] AudioClip[] zombieBreathingSound; 

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    EnemyHealth health;
    Transform target;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
        zombieSfxSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health.IsDead())
        {
            enabled = false;
            //navMeshAgent.enabled = false; TODO: causing error when enemy is killed
            
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
        } else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    public void PlayZombieSound() {
        zombieSfxSource.loop = true;
        int zombieBreathingSoundLength = zombieBreathingSound.Length;

        if (zombieBreathingSoundLength > 0)
        {
            int randomIndex = Random.Range(0, zombieBreathingSoundLength);
            AudioSource.PlayClipAtPoint(zombieBreathingSound[randomIndex], transform.position);
        }
        else {
            Debug.LogWarning("No zombie breathing sounds assigned");
        }
        

    }

    public void OnDamageTaken()
    {
        isProvoked = true;
        //zombieSfxSource.PlayOneShot(zombieTakeDamage);
        GetComponent<Animator>().SetTrigger("Knock_Back");
        //Debug.Log("Damage taken on enemy");
    }

    [SerializeField] float lastSoundPlayTime = 0f;
    [SerializeField] float soundCooldown = 5f;


    private void EngageTarget()
    {
        FaceTarget();

        if (Time.time - lastSoundPlayTime > soundCooldown && distanceToTarget <= chaseRange / 2)
        {
            PlayZombieSound();
            lastSoundPlayTime = Time.time;
        }

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("Attack", false);
        GetComponent<Animator>().SetTrigger("Move");
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("Attack", true);
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }


}
