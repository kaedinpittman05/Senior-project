using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health = 6f;
    public GameObject hearts;
    private float timeLastHit = -1.5f;
    private float timeLastFlashed;
    private bool spritesDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FlashOnHit();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && CheckIfVulnerable())
        {
            health = health - 1;
            hearts.GetComponent<HealthTracking>().Damage();
            Debug.Log(health);
            timeLastHit = Time.time;
            Debug.Log(timeLastHit);
            if (health <= 0)
            {
                SceneManager.LoadScene("main_menu");
            }
        }
    }

    private void FlashOnHit()
    {
        if (!CheckIfVulnerable())
        {
            if (!spritesDisabled && timeLastFlashed <= Time.time + 0.15)
            {
                foreach (SpriteRenderer i in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    i.enabled = false;
                }
                spritesDisabled = true;
                timeLastFlashed = Time.time;
            }
            else if (spritesDisabled && timeLastFlashed <= Time.time + 0.15)
            {
                foreach (SpriteRenderer i in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    i.enabled = true;
                }
                spritesDisabled = false;
                timeLastFlashed = Time.time;
            } 
        }
        else
        {
            foreach (SpriteRenderer i in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                i.enabled = true;
            }
            spritesDisabled = false;
        }
    }

    private bool CheckIfVulnerable()
    {
        bool isVulnerable;
        if (timeLastHit <= Time.time - 1.5)
        {
            isVulnerable = true;
        }
        else
        {
            isVulnerable = false;
        }
        return isVulnerable;
    }
}
