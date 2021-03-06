﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public int maxHP;
	public int hitPoints;
	public int lives;
	public float timeToDestroy;
	private GameObject spawn;
	public GameObject[] spawnPoints;
	private float distanceFromPlayer;
	private int closestSpawn;
	private float tmpDistance;
	private float tmpX;
	private float tmpY;
	public bool player1Died;
	public bool player2Died;
	private Transform player1, player2, entity;
    private float timerDmg;

    public Image[] hearts, livesDisplay;
    public Sprite fullHeart;


    void Start()
	{
		hitPoints = maxHP;
		player1 = Camera.main.GetComponent<CameraScript>().player1;
		player2 = Camera.main.GetComponent<CameraScript> ().player2;
        timerDmg = 0f;
	}
    
	public int HitPoints
	{
		get { return hitPoints; }
		set
		{
			hitPoints += value;
			if (hitPoints > maxHP)
			{
				hitPoints = maxHP;
			}
		}

	}
    
	public void removeHitPoints(int i)
	{
        if (timerDmg > 1f)
        {
            hitPoints -= i;
            Debug.Log(gameObject.tag + "'s HP is at " + hitPoints);
            timerDmg = 0;
        }

	}

	IEnumerator Death()
	{
		yield return new WaitForSeconds (timeToDestroy);

		if (gameObject.tag == "Player")
		{
			--lives;
			if (lives <= 0)
			{
				/*
				if (gameObject.transform == Camera.main.GetComponent<CameraScript> ().player1)
				{
					Camera.main.GetComponent<CameraScript> ().SetPlayer1 = Camera.main.GetComponent<CameraScript> ().player2;

				}
				else if (gameObject.transform == Camera.main.GetComponent<CameraScript> ().player2)
				{
					Camera.main.GetComponent<CameraScript> ().SetPlayer2 = Camera.main.GetComponent<CameraScript> ().player1;
				}
				*/
				//WE SHOULD ADD A GAMEOVER CHECK TO SEE IF BOTH PLAYERS ARE DEAD.
				hearts[0].enabled = false;
				hearts[1].enabled = false;
				hearts[2].enabled = false;

				livesDisplay [0].enabled = false;
				Destroy (gameObject);
			}
			else
			{
				hitPoints = maxHP;
				//SPAWNS BASED ON THE POSITION OF THE PLAYER, WILL FIND CLOSEST SPAWN POINT FROM DEATH
				//NEEDS TO BE MODIFYIED TO BASE SPAWN POINT ON PLAYER 2 POSITION
				//SPAWNER NEEDS TO SPAWN EITHER PLAYER 1 OR PLAYER 2 NOT JUST GAMEOBJECT GIVEN

				if (gameObject.transform == player1)
				{
					player1Died = true;
					if (player2 == null)
					{
						entity = player1;
					}
					else
					{
						entity = player2;
					}	
				}
				else
				{
					player2Died = true;
					if (player1 == null)
					{
						entity = player2;
					}
					else
					{
						entity = player1;
					}	
				}
				FindSpawn (entity);
			}
		}
		else
		{
			if (gameObject.tag == "Wizard")
			{
				Destroy (gameObject);
				FindObjectOfType<GameManager> ().Congratulations ();
                FindObjectOfType<Timer> ().StopTimer();
			}
			else
			{
				Destroy (gameObject);
			}

		}
	}

    public void FindSpawn(Transform obj)

    {

        /*

		if (FindObjectOfType<Lava> ().lavaIsActive == true)

		{

			spawnCushion = 7.0f;

		}

		else

		{

			spawnCushion = 2.0f;

		}

		*/



        tmpX = Mathf.Abs(obj.position.x - spawnPoints[0].transform.position.x);

        tmpY = Mathf.Abs(obj.position.y + 3.0f - spawnPoints[0].transform.position.y);

        tmpDistance = Mathf.Sqrt(Mathf.Pow(tmpX, 2) + Mathf.Pow(tmpY, 2));

        distanceFromPlayer = tmpDistance;

        closestSpawn = 0;



        for (int i = 1; i < spawnPoints.Length; ++i)

        {

            tmpX = Mathf.Abs(obj.position.x - spawnPoints[i].transform.position.x);

            tmpY = Mathf.Abs(obj.position.y + 3.0f - spawnPoints[i].transform.position.y);      //provide 3.0 units of cushion 



            tmpDistance = Mathf.Sqrt(Mathf.Pow(tmpX, 2) + Mathf.Pow(tmpY, 2));



            if (tmpDistance < distanceFromPlayer)

            {

                distanceFromPlayer = tmpDistance;

                closestSpawn = i;

            }

        }

        if (obj.position.y < spawnPoints[closestSpawn].transform.position.y) //spawn is above player

        {

            if (FindObjectOfType<Lava>().lavaIsActive == true)

            {

                //checks if covered by lava or spawning right in front of it

                if (spawnPoints[closestSpawn].transform.position.y >= FindObjectOfType<Lava>().transform.position.y + 16.5f

                    || spawnPoints[closestSpawn].transform.position.y < FindObjectOfType<Lava>().transform.position.y - 16.5f)

                {

                    if (closestSpawn % 2 == 1)

                    {

                        closestSpawn = closestSpawn + 2; //spawn a point above on right

                    }

                    else

                    {

                        ++closestSpawn; //spawn a point below on left

                    }

                }

                else

                {

                    Debug.Log("No change, spawn at closest.");

                }

            }

            else

            {

                if (closestSpawn % 2 == 1)

                {

                    closestSpawn = closestSpawn - 2; //spawn a point below on right

                }

                else

                {

                    --closestSpawn; //spawn a point below on left

                }

            }

        }

        Spawn respawn = spawnPoints[0].GetComponent<Spawn>();

        if (closestSpawn < 0)

        {

            respawn = spawnPoints[0].GetComponent<Spawn>();

        }

        else

        {

            respawn = spawnPoints[closestSpawn].GetComponent<Spawn>();

        }

        respawn.Respawn();

    }

    public GameObject Spawn
    {
        get { return spawn; }
        set
        {
            spawn = value;
        }
    }

    void Update()
	{
        timerDmg += Time.deltaTime;

        for (int i = 0; i < hearts.Length; i++)
        {
            /*
            if (hitPoints > maxHP)
            {
                hitPoints = maxHP;
            }
            if (i < hitPoints)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
            	Debug.Log("No Hearts");
                // hearts[i].sprite = emptyHeart;
            }
            */
            if (i < hitPoints)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        for (int i = 0; i < livesDisplay.Length; i++)
        {
            if (i < lives)
            {
                livesDisplay[i].enabled = true;
            }
            else
            {
                livesDisplay[i].enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (hitPoints <= 0)
        {
            StartCoroutine(Death());
            print("death");
        }
    }
}
