using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement2 : MonoBehaviour
{
    private float Distance;
    private float chaseRange = 20;
    private float attackRange = 1f;
    private float attackRepeatTime = 1;
    private float enemyHealth = 40;
    private bool isDead;
    private float attackTime;
    private int damage = 15;
    private Animation animations;
    public AudioClip deathClip;
    public AudioClip hurtClip;
    PlayerController playerHealth;
    public int scoreValue = 50;

    AudioSource enemyAudio;


    Transform player;
    UnityEngine.AI.NavMeshAgent nav;

    // Start is called before the first frame update
    void Awake()
    {
        animations = gameObject.GetComponent<Animation>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GetComponent<PlayerController>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        attackTime = Time.time;
        enemyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Distance = Vector3.Distance(player.position, transform.position);

            if (Distance > chaseRange)
            {
                idle();
            }

            if (Distance < chaseRange && Distance > attackRange)
            {
                chase();
            }

            if (Distance < attackRange)
            {
                attack();
            }
        }
    }

    void idle()
    {
        animations.Play("Idle");
    }

    void chase()
    {
        animations.Play("Combat_run");
        nav.destination = player.position;
    }

    void attack()
    {
        nav.destination = transform.position;

        if (Time.time > attackTime)
        {
            animations.Play("Attack");
            player.GetComponent<PlayerController>().ApplyDamage(damage);
            attackTime = Time.time + attackRepeatTime;
        }
    }

    public void ApplyDamage(int damage)
    {
        if (!isDead)
        {
            enemyAudio.clip = hurtClip;
            enemyAudio.Play();
            enemyHealth = enemyHealth - damage;

            if (enemyHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        isDead = true;
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
        animations.Play("Death");
        ScoreManager.score += scoreValue;
        Destroy(transform.gameObject, 10);
    }
}
