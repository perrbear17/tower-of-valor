﻿using UnityEngine;

public class Throwable : MonoBehaviour {

    public bool isGrabbed;
    public bool isThrown;
    public KeyCode left, right;
    public int maxStruggle;
    private static int strugglePoints;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update () {

        if (isGrabbed)
        {
            Struggle();
            DisableMovement();
        }
    }

    void LateUpdate()
    {
        if (strugglePoints >= maxStruggle)
        {
            EnableMovement();
        }

    }

    // player can press keys x amount of times to get out of grab
    void Struggle()
    {
        if (Input.GetKeyDown(left))
        {
            body.velocity = new Vector2(-10, body.velocity.y);
            strugglePoints++;
        }
        if (Input.GetKeyDown(right))
        {
            body.velocity = new Vector2(10, body.velocity.y);
            strugglePoints++;
        }

        // acquired enough struggle points to escape grab
        if (strugglePoints >= maxStruggle)
        {
            // jumps a bit
            body.velocity = new Vector2(body.velocity.x, 10);

            isGrabbed = false;

            EnableMovement();
        }
    }

    // disable movement script
    public void DisableMovement()
    {
        GetComponent<playerMovement>().enabled = false;  
    }

    public void EnableMovement()
    {
        GetComponent<playerMovement>().enabled = true;
    }



    // Object regains movement if collided with something after being thrown
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.root.tag == "Platform")
        {
            isGrabbed = false;
            EnableMovement();
        }

        if (isThrown)
        {
            EnableMovement();
            isThrown = false;
            GetComponent<Rigidbody2D>().mass = 1;
        }
    }


    public void ResetStrugglePoints()
    {
        strugglePoints = 0;
    }
}
