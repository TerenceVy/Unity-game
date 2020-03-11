﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{

    //Animation
    Animation animations;

    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public string inputAttack;

    public Vector3 jumpSpeed;
    CapsuleCollider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        animations = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    bool OnTerrain()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.08f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
            animations.Play("run");
        }

        if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, 0, runSpeed * Time.deltaTime);
            animations.Play("run");
        }

        if (Input.GetKey(inputBack))
        {
            transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
            animations.Play("walk");
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
            animations.Play("idlebattle");
        }

        if (Input.GetKey(inputAttack))
        {
            animations.Play("attack");
        }

        if (Input.GetKeyDown(KeyCode.Space) && OnTerrain())
        {
            Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
            v.y = jumpSpeed.y;

            gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
        }
    }
}
