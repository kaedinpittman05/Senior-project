using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health = 6f;
    public GameObject hearts; // The healthbar image to update upon taking damage
    private float timeLastHit = -1.5f;
    private float timeLastFlashed;
    public float timeInvulnerable = 1.5f;
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

    // Applies damage logic when the player collides with an enemy or boss
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && CheckIfVulnerable() || other.gameObject.tag == "Boss" && CheckIfVulnerable())
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

    // Causes the player sprites to flash while the player is in post-hit invulnerability
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

    // Checks if the player has taken damage recently in order to keep track of when they should be invulnerable
    private bool CheckIfVulnerable()
    {
        bool isVulnerable;
        if (timeLastHit <= Time.time - timeInvulnerable)
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
