﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float chargeDelay = 0.3f;
    private float chargeup;


    private Rigidbody2D rb;
    private Weapon weapon;
    private Animator anim;
    private Player player;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private bool charged = false;
    private bool charging = false;
    //Weapon Fire
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.Play("Fire", -1, 0);
            weapon.Fire("Player",Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButton(0))
        {
           
            chargeDelay -= Time.deltaTime;
            if(chargeDelay <= 0)
            {
                if (!charging)
                {
                    AudioManager.instance.PlaySound("Charging");
                    charging = true;
                }
               
                chargeup += Time.deltaTime;
                player.cam.Shake(0.1f, 0.1f);
            } else
            {
                charging = false;
            }

            if (chargeup >= weapon.chargeTime && !charged)
            {
                charged = true;
                AudioManager.instance.PlaySound("Charged");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(chargeup >= weapon.chargeTime)
            {
                anim.Play("Fire", -1, 0);
                weapon.ChargedShot("Player", Camera.main.ScreenToWorldPoint(Input.mousePosition));
                player.cam.Shake(0.5f, 0.5f);
                charged = false;
            }

            chargeup = 0;
            chargeDelay = 0.3f;
        }
    }

    public void FixedUpdate()
    {
        Move();
        // convert mouse position into world coordinates
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseWorldPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }

    //Move
    public void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(x * moveSpeed, y * moveSpeed);
        rb.velocity = movement;
    }
}
