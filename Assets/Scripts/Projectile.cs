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

    void Move()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void TryDestroy()
    {
        if (this.GetComponent<CircleCollider2D>().IsTouching(target))
        {
            Destroy(this.gameObject);
            gameObject.SetActive(false);
            Debug.Log("Destroy");
            
        }
        else if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
            Debug.Log("Destroy");
        }
    }

    void SpeedUp()
    {
        if (Time.time >= (destroyTime / 2) && canSpeedUp)
        {
            speed = speed * 2;
            canSpeedUp = false;
        }
    }

    void OnSpawn()
    {
        if (justSpawned)
        {
            speed = 3;
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
            justSpawned = false;
        }
    }
}
