﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FacingDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}


/// <summary>
/// Author: Andrew Seba
/// Description: Player Controller Script
/// </summary>
public class Player : MonoBehaviour {

    public GameObject sprite;
    public GameObject sword;
    public float speed = 2f;
    public float hurtAgainDelay = 1f; //Seconds
    public float hitForce = 100f;
    public GameObject winMenu;
    public GameObject looseMenu;

    [Header("Audio Settings")]
    public AudioClip hurtClip;

    private FacingDirection facing = FacingDirection.DOWN;
    private Animator playerAnimator;
    private Animator swordAnimator;
    private PlayerHealth health;
    private AudioSource audioSource;
    private float horizontal = 0;
    private float vertical = 0;
    private bool lockDirection = false;
    private bool canGetHurt = true;

    private void Start()
    {
        playerAnimator = sprite.GetComponent<Animator>();
        swordAnimator = sword.GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame


    void Update ()
    {
        GetInput();

        switch (facing)
        {
            case FacingDirection.UP:
                playerAnimator.SetInteger("Facing", 0);
                break;
            case FacingDirection.DOWN:
                playerAnimator.SetInteger("Facing", 1);
                break;
            case FacingDirection.LEFT:
                playerAnimator.SetInteger("Facing", 2);
                break;
            case FacingDirection.RIGHT:
                playerAnimator.SetInteger("Facing", 3);
                break;
            default:
                break;
        }

    }

    void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKey("right") || horizontal > 0 && !Input.GetKey("left"))
        {
            horizontal = 1;
            if(!lockDirection)
                facing = FacingDirection.RIGHT;
        }
        if (Input.GetKey("left") || horizontal < 0 && !Input.GetKey("right"))
        {
            horizontal = -1;
            if (!lockDirection)
                facing = FacingDirection.LEFT;
        }
        if (Input.GetKey("up") || vertical > 0 && !Input.GetKey("down"))
        {
            vertical = 1;
            if (!lockDirection)
                facing = FacingDirection.UP;
        }
        if (Input.GetKey("down") || vertical < 0 && !Input.GetKey("up"))
        {
            vertical = -1;
            if (!lockDirection)
                facing = FacingDirection.DOWN;
        }

        //Button UP
        if (Input.GetButtonUp("Fire1"))
            ResetAttack();
        //Button Down
        if (Input.GetButtonDown("Fire1"))
            Attack();
        if(Input.GetButtonUp("Fire1"))
            lockDirection = false;

    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(horizontal, vertical).normalized * Time.deltaTime * speed;
    }

    private void Attack()
    {
        swordAnimator.SetBool("attack", true);
        sword.GetComponent<BoxCollider2D>().enabled = true;
        lockDirection = true;
    }
    
    public void ResetAttack()
    {
        swordAnimator.SetBool("attack", false);
        sword.GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (canGetHurt)
            {
                audioSource.clip = hurtClip;
                audioSource.Play();
                health.Hurt(other.GetComponent<Enemy>().attackDamage);
                if(health.health <= 0)
                {
                    looseMenu.SetActive(true);
                }
                canGetHurt = false;
                Invoke("ResetHurt", hurtAgainDelay);
                playerAnimator.SetTrigger("Hurt");
                Vector2 direction = transform.position - other.transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().AddForce(direction * hitForce);
            }
        }
        if(other.tag == "Objective")
        {
            Time.timeScale = 0;
            winMenu.SetActive(true);
        }
    }

    void ResetHurt()
    {
        canGetHurt = true;
    }
}
