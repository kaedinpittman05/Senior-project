using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracking : MonoBehaviour
{
    public int health;
    public Sprite sixHealth;
    public Sprite fiveHealth;
    public Sprite fourHealth;
    public Sprite threeHealth;
    public Sprite twoHealth;
    public Sprite oneHealth;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BattleTheme");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeHealthSprite();
    }

    void ChangeHealthSprite()
    {
        switch (health)
        {
            case 6:
                this.gameObject.GetComponent<Image>().sprite = sixHealth;
                break;
            case 5:
                this.gameObject.GetComponent<Image>().sprite = fiveHealth;
                break;
            case 4:
                this.gameObject.GetComponent<Image>().sprite = fourHealth;
                break;
            case 3:
                this.gameObject.GetComponent<Image>().sprite = threeHealth;
                break;
            case 2:
                this.gameObject.GetComponent<Image>().sprite = twoHealth;
                break;
            case 1:
                this.gameObject.GetComponent<Image>().sprite = oneHealth;
                break;
            default:
                Debug.Log("You either have 0 or less health, or something broke.");
                break;
        }
    }

    public void Damage()
    {
        health--;
    }
}
