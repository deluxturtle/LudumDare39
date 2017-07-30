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

    public int attackDamage = 1;
    public float activationRange = 10f;

    private int health = 3;
    private Transform player;
    private bool waiting = true;
    private bool activated = false;


    EnemyState state = EnemyState.Wander;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine("Activation");
    }

    public void Hurt(int damage)
    {
        Debug.Log("Got Hurt");
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
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
            yield return null;
        }
    }


}
