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

    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + 5;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = FindObjectOfType<PlayerMovement>().transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
            Debug.Log("Destroy");
        }
        else if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
            Debug.Log("Destroy");
        }
    }
}
