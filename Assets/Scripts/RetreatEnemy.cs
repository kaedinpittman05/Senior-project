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
    public float timeBetweenShots;
    private float nextShotTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= activeDistance)
        {
            if (Time.time > nextShotTime)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                nextShotTime = Time.time + timeBetweenShots;
            }

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

    private void Move()
    {
        if (Vector2.Distance(transform.position, target.position) < minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, target.position) > maxDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
