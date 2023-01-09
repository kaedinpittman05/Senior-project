using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   //Changes scene to the next
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);



    }
    // Closes game
    public void QuiteGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


}
