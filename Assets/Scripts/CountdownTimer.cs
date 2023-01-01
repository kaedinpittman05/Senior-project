using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    public float currentTime = 0f;
    float startingTime = 10f;

    [SerializedField]Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 2 * currentTime.deltaTime;
        countdownText.text = currentTime.ToString("0");
        
        if(currentTime <= 0)
        {
            currentTime = 0;
        }
    }
}
