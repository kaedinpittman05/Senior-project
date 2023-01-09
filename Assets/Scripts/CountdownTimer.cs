using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    public float currentTime = 0f;
    public float startingTime = 10f;

    [SerializeField] Text countdownText;

    //Sets current time
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Counts up and adds it to the text box
        currentTime += 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");
        
        if(currentTime <= 0)
        {
            currentTime = 0;
        }
    }
}
