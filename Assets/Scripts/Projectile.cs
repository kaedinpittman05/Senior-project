using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Vector3 targetPosition;
    public float speed;
    private float destroyTime;
    private bool canSpeedUp = true;
    private bool justSpawned = true;
    BoxCollider2D target;

    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + 5;
    }

    // Update is called once per frame
    void Update()
    {

        OnSpawn();

        Move();

        TryDestroy();

        SpeedUp();
    }

    // Moves the projectile towards the first object it finds with tag "Player"
    void Move()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    // Destroys the projectile if either of its destruction conditions have been met
    void TryDestroy()
    {
        if (this.GetComponent<CircleCollider2D>().IsTouching(target)) // Destroy this object if it is touching its target
        {
            Destroy(this.gameObject);
            gameObject.SetActive(false);
            //Debug.Log("Destroy");
            
        }
        else if (Time.time >= destroyTime) // Destroy this object if it has existed for its destroy time
        {
            Destroy(gameObject);
            //Debug.Log("Destroy");
        }
    }

    // Increases the speed of this object when it has existed for half of its destroy time
    void SpeedUp()
    {
        if (Time.time >= (destroyTime / 2) && canSpeedUp)
        {
            speed = speed * 2;
            canSpeedUp = false;
        }
    }

    // Sets the target of this object when it is initially created
    void OnSpawn()
    {
        if (justSpawned)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
            justSpawned = false;
        }
    }
}
