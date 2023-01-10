using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public float currentTime = 0f;
    [SerializeField] Text countdownText;
    public float startingTime = 0f;
  

    // Update is called once per frame
    void Update()
    {

        //If games is paused increase time and add to text

        if (GameIsPaused == false)
        {
            currentTime += 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
            }

        }

        //If escape is pressed do pause if not else resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Start()
    {
        currentTime = startingTime;
        Resume();
    }

    //Resumes game
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
       

    }

    //Pauses game and brings up pause menu.
    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }
    //Returns to Menu
   public void ReturnMenu ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        FindObjectOfType<AudioManager>().StopPlaying("BattleTheme");

    }
}
