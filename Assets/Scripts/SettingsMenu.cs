using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.Diagnostics;


public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    public Slider musicSlider;
    public Slider soundSlider;
    private string dbName = "game_database";
    IDbConnection connection;


    Resolution[] resolutions;

    void Start ()
    {
        connection = new SqliteConnection(string.Format("URI=file:Assets/Streaming Assets/{0}.db", dbName));


        resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


    }


  

    public void setResolution   (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


   public void SetMusicVolume (float volume)
    {
        
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);

    



    }


   public void SetSoundVolume(float volume)
   {
        audioMixer.SetFloat("soundsVolume", Mathf.Log10(volume) * 20);



    }

    public void SetQuality (int qualityIndex)
    {
        
        QualitySettings.SetQualityLevel(qualityIndex);


    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    void PushCommand(string commandString, IDbConnection connection)
    {
        // Create new command
        IDbCommand command = connection.CreateCommand();
        // Add your comment text (queries)
        command.CommandText = string.Format("{0}", commandString);
        // Execute command reader - execute command
        command.ExecuteReader();
    }



    public void MusicSave()
    {
        //connection.Open();
        //PushCommand(string.Format("UPDATE Settings SET music = {0} WHERE Current = 1;", volume), connection);
        
        
        


        float music = musicSlider.value;
        float sound = soundSlider.value;
        UnityEngine.Debug.Log(music);
        UnityEngine.Debug.Log(sound);
        //connection.Close();


    }

}
