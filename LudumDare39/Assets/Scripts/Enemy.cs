using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Agro
}

/// <summary>
/// Author: Andrew Seba
/// Description: Basic enemy that hurts you if you touch it.
/// </summary>
public class Enemy : MonoBehaviour {

    public float moveSpeed = 2;
    public int attackDamage = 1;
    public float activationRange = 10f;
    public float agroRange = 5f;
    public float hurtResetTime = 0.5f;
    public AudioClip hurt;

    private AudioSource audioSource;
    private Transform player;
    private Animator animator;
    private int health = 3;
    private bool waiting = true;
    private bool activated = false;
    private bool canGetHurt = true;


    EnemyState state = EnemyState.Wander;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("Activation");
    }

    public void Hurt(int damage)
    {
        if (canGetHurt)
        {
            if(hurt != null)
            {
                audioSource.clip = hurt;
                audioSource.Play();
            }
            canGetHurt = false;
            GetComponent<Collider2D>().enabled = false;
            health -= damage;
            animator.SetTrigger("hurt");
            if (health <= 0)
            {
                Destroy(gameObject, 1);
            }
            else
            {
                Invoke("ResetHurt", hurtResetTime);
            }
        }

    }

    void ResetHurt()
    {
        canGetHurt = true;
        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator Activation()
    {
        while (true)
        {
            if(!activated && Vector2.Distance(transform.position, player.position) < activationRange)
            {
                Debug.Log("Activated:" + gameObject.name);
                activated = true;
                StartCoroutine("Wander");
                
            }
            else if(Vector2.Distance(transform.position, player.position) > activationRange)
            {
                activated = false;
                StopCoroutine("Wander");
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Wander()
    {
        Debug.Log("Wandering");
        while (true)
        {
            if(Vector2.Distance(player.position, transform.position) < agroRange)
            {
                StartCoroutine("Agro");
                break;
            }
            yield return null;
        }
    }

    IEnumerator Agro()
    {
        while (true)
        {
            if(canGetHurt && player.GetComponent<PlayerHealth>().health > 0)
                transform.Translate((player.position - transform.position).normalized * Time.deltaTime * moveSpeed);
            yield return null;
        }
    }


}
