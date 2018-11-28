﻿using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour {

    public KeyCode jump;
    public float jumpHeight;
    private Rigidbody2D body;

    public Animator animation;

    public bool isGrounded;
    public bool canJump;
    public bool canDoubleJump;
    public bool doubleJumpPower;
	public bool jumpedOnBat;
	private bool landedOnPlayer;
	public float stunDelay;

    public Transform leftFoot, rightFoot, leftHeel, rightHeel;

    private RaycastHit2D hitLeftFoot, hitLeftHeel, hitRightFoot, hitRightHeel;
	public float feetCheck;

    // Use this for initialization
    void Start () 
	{
        body = GetComponent<Rigidbody2D>();
        canJump = true;
	}
	// Update is called once per frame
	void LateUpdate () 
	{
        if (Input.GetKeyDown(jump))
        {
            if (isGrounded)
            {
                // Jump
                body.velocity = new Vector2(body.velocity.x, jumpHeight);
                isGrounded = false;
                animation.SetFloat("Vertical", body.velocity.x); // jump animation
            }
            // on air
            else
            {
                if (canDoubleJump && doubleJumpPower)
                {
                    // Jump
                    body.velocity = new Vector2(body.velocity.x, jumpHeight);
                    canDoubleJump = false;
                }
            }
        }
    }
    /** While Colliding with object -- should check if lands on enemy
										if bat, kill bat
										else if enemy, player takes 1 damage
	**/
	void OnCollisionEnter2D(Collision2D col)
	{
		// Check if gameobject has an Enemy script
		if (col.gameObject.tag == "Bat")
		{
			CheckBottomRaycast ();
			//Check if grounded
			if (isGrounded)
			{
				Debug.Log ("Player is grounded");
			}
			else if (!hitLeftFoot.collider && !hitLeftHeel.collider && !hitRightFoot.collider && !hitRightHeel.collider)
			{
				Debug.Log ("Feet Raycast NOT detecting a collider.");
			}
			else
			{
				if (hitLeftFoot.collider)
				{
					if (hitLeftFoot.collider.gameObject.tag == "Bat")
					{
						jumpedOnBat = true;
					}
				}
				else if (hitLeftHeel.collider)
				{
					if (hitLeftHeel.collider.gameObject.tag == "Bat")
					{
						jumpedOnBat = true;
					}
				}
				else if (hitRightFoot.collider)
				{
					if (hitRightFoot.collider.gameObject.tag == "Bat")
					{
						jumpedOnBat = true;
					}
				}
				else if (hitRightHeel.collider)
				{
					if (hitRightHeel.collider.gameObject.tag == "Bat")
					{
						jumpedOnBat = true;
					}
				}
				if (jumpedOnBat)
				{
					Debug.Log ("Jumped on Bat");
					col.collider.gameObject.GetComponent<Health> ().removeHitPoints (1);
					FindObjectOfType<AudioManager> ().Play ("SquishedBat");
				}
			}
		}
		else if (col.gameObject.tag == "Platform")
		{
			isGrounded = true;
			//canDoubleJump = false;
		}
		else if (col.gameObject.tag == "Player")
		{
			CheckBottomRaycast ();
			//Check if grounded
			if (isGrounded)
			{
				Debug.Log ("Player is grounded");
			}
			else if (!hitLeftFoot.collider && !hitLeftHeel.collider && !hitRightFoot.collider && !hitRightHeel.collider)
			{
				Debug.Log ("Feet Raycast NOT detecting a collider.");
			}
			else
			{
				if (hitLeftFoot.collider)
				{
					if (hitLeftFoot.collider.gameObject.tag == "Player")
					{
						landedOnPlayer = true;
					}
				}
				else if (hitLeftHeel.collider)
				{
					if (hitLeftHeel.collider.gameObject.tag == "Player")
					{
						landedOnPlayer = true;
					}
				}
				else if (hitRightFoot.collider)
				{
					if (hitRightFoot.collider.gameObject.tag == "Player")
					{
						landedOnPlayer = true;
					}
				}
				else if (hitRightHeel.collider)
				{
					if (hitRightHeel.collider.gameObject.tag == "Player")
					{
						landedOnPlayer = true;
					}
				}
				if (landedOnPlayer)
				{
					GameObject stunnedPlayer = col.collider.gameObject;
					Debug.Log ("Jumped on Player. PLAYER IS STUNNED");
					FindObjectOfType<AudioManager> ().Play ("Bonk");
					StartCoroutine (Stun (stunnedPlayer));
					landedOnPlayer = false;
				}
			}
		}
	}

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckBottomRaycast();
        if (hitLeftFoot == true || hitRightFoot == true || hitLeftHeel == true || hitRightFoot == true )
        {
            isGrounded = true;
            canDoubleJump = true;
        }
    }
    // Cast 4 downward raycasts on all 4 feet and heel positions
    void CheckBottomRaycast()
    {       
        // check if bottom collider rays hit something
        hitLeftFoot = Physics2D.Raycast(leftFoot.position, Vector2.down, feetCheck);
		hitRightFoot = Physics2D.Raycast(rightFoot.position, Vector2.down, feetCheck);
		hitLeftHeel = Physics2D.Raycast(leftHeel.position, Vector2.down, feetCheck);
		hitRightHeel = Physics2D.Raycast(rightHeel.position, Vector2.down, feetCheck);
    }
    // When Object doesn't collide anymore
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

	IEnumerator Stun(GameObject prey)
	{
		float tmp = prey.GetComponent<playerMovement> ().moveSpeed;
		prey.GetComponent<playerMovement> ().moveSpeed = 0;

		yield return new WaitForSeconds (stunDelay);

		prey.GetComponent<playerMovement> ().moveSpeed = tmp;
	}
}