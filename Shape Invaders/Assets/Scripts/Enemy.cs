﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float fireDelay;
    public float shootRange = 10f;

    [HideInInspector]
    public float fireCountdown;

    [HideInInspector]
    public bool ragdoll;

    public Weapon weapon;

    private Transform player;
    private Vector3 dir;

    private Rigidbody2D rb;
    private Animator anim;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        fireCountdown = fireDelay;
    }

    public virtual void Update()
    {
        if (InRange())
        {
            fireCountdown -= Time.deltaTime;
        }
    
        if(fireCountdown <= 0)
        {
            Fire();
            fireCountdown = fireDelay;
        }
    }

    public virtual void FixedUpdate()
    {
        Move();    
    }

    public virtual void LoseSpeed(float strength)
    {
        moveSpeed -= strength;
        if(moveSpeed <= 0)
        {
            moveSpeed = 0;
        }
    }

    public virtual IEnumerator Ragdoll(float duration)
    {
        ragdoll = true;
        yield return new WaitForSeconds(duration);
        ragdoll = false;
    }

    public void FindPlayer()
    {
        dir = player.position - transform.position;
        dir = dir.normalized;
    }

    public void Aim()
    {
        // convert mouse position into world coordinates
        Vector2 playerPosition = player.transform.position;

        // get direction you want to point at
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }

    public void Fire()
    {
        weapon.Fire(player.transform.position);
        anim.Play("Shoot", -1, 0);
    }

    public void Move()
    {
        if (ragdoll) return;

        if (player == null) return;

        FindPlayer();

        if (!InRange())
        {
            rb.velocity = dir * moveSpeed;
        } else
        {
            rb.velocity = new Vector2(0, 0);
        }
    
        Aim();
    }

    public void Charge()
    {
        rb.AddForce(dir * moveSpeed * 2,ForceMode2D.Impulse);
    }

    public  bool InRange()
    {
        return Physics2D.OverlapCircle(transform.position, shootRange, LayerMask.GetMask("Player"));
    }
}
