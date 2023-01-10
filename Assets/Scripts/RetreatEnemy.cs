using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float minDistance;
    public float maxDistance;
    public float activeDistance;

    public GameObject projectile;
    public float timeBetweenShots; // Used as the interval to calculate time to next shoot
    private float nextShotTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= activeDistance) // Run the next segment of code if the player has entered the enemy's range of activity
        {

            Shoot();

            if (target == null)
            {
                SetTarget();
            }
            else
            {
                Move();
            } 
        }


    }

    // Move this object
    private void Move()
    {
        if (Vector2.Distance(transform.position, target.position) < minDistance) // If the player is too close to this object, move away from the player
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, target.position) > maxDistance) // If the player is too far away from this object, move towards the player
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    // Sets this object's target variable to the first object with tag "Player"
    private void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Creates a projectile at this object's location and updates nextShotTime
    private void Shoot()
    {
        if (Time.time >= nextShotTime)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }
    }
}
