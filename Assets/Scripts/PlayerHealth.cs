using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health = 6f;
    public GameObject hearts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            health = health - 1;
            hearts.GetComponent<HealthTracking>().Damage();
            Debug.Log(health);
            if (health <= 0)
            {
                SceneManager.LoadScene("main_menu");
            }
        }
    }
}
