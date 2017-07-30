using System.Collections;
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

    private FacingDirection facing = FacingDirection.DOWN;
    private Animator playerAnimator;
    private Animator swordAnimator;
    private float horizontal = 0;
    private float vertical = 0;
    private bool lockDirection = false;

    private void Start()
    {
        playerAnimator = sprite.GetComponent<Animator>();
        swordAnimator = sword.GetComponent<Animator>();
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
        lockDirection = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            
        }
    }
}
