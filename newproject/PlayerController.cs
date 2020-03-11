using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    Animation animations;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public Vector3 jumpSpeed;
    public float attackCooldown;
    public bool isAttacking;
    public float attackRange;
    CapsuleCollider playerCollider;
    public bool isDead = false;
    Image hpImage;
    float maxHealth = 100;
    public float currentHealth = 100;
    int currentDamage = 10;
    private float currentCooldown;
    public GameObject rayHit;
    public AudioClip deathClip;
    public AudioClip attackClip;
    public AudioClip missAttackClip;
    public AudioClip hurtClip;

    AudioSource playerAudio;


    void Start()
    {
        animations = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        hpImage = GameObject.Find("currentHP").GetComponent<Image>();
        rayHit = GameObject.Find("RayHit");
        StartCoroutine(RenegerateHealth());
        playerAudio = GetComponent<AudioSource>();

    }

    bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.09576419f, layerMask:3);
    }

    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, walkSpeed * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }

            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, runSpeed * Time.deltaTime);
                animations.Play("run");
            }

            if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }

            if (Input.GetKey(inputLeft))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(inputRight))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }

            if (!Input.GetKey(inputFront) && !Input.GetKey(inputBack))
            {
                if (!isAttacking)
                {
                    animations.Play("idle");
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
                v.y = jumpSpeed.y;
                gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
            }

            float percentageHP = ((currentHealth * 100) / maxHealth) / 100;
            hpImage.fillAmount = percentageHP;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (isAttacking)
        {
            currentCooldown -= Time.deltaTime;
        }
        if(currentCooldown <= 0)
        {
            currentCooldown = attackCooldown;
            isAttacking = false;
        }
    }

    public void ApplyDamage(float Damage)
    {
        if (!isDead)
        {
            playerAudio.clip = hurtClip;
            playerAudio.Play();
            currentHealth = currentHealth - Damage;

            if (currentHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        playerAudio.clip = deathClip;
        playerAudio.Play();
        isDead = true;
        animations.Play("die");
    }

    public void Attack()
    {
        if (!isDead)
        {
            if (!isAttacking)
            {
                animations.Play("attack");

                RaycastHit hit;
                if (Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
                {
                    Debug.DrawLine(rayHit.transform.position, hit.point, Color.red);
                    if (hit.transform.tag == "Enemy")
                    {
                        playerAudio.clip = attackClip;
                        playerAudio.Play();
                        hit.transform.GetComponent<enemyMovement>().ApplyDamage(currentDamage);
                    }
                    if (hit.transform.tag == "Boss")
                    {
                        playerAudio.clip = attackClip;
                        playerAudio.Play();
                        hit.transform.GetComponent<enemyMovement2>().ApplyDamage(currentDamage);
                    }
                }
                else
                {
                    playerAudio.clip = missAttackClip;
                    playerAudio.Play();
                }
                isAttacking = true;
            }
        }
    }

    IEnumerator RenegerateHealth()
    {
        while(!isDead)
        {
            currentHealth = Mathf.Clamp(currentHealth + 1, -100, 100);
            yield return new WaitForSeconds(1);
        }
    }
}

