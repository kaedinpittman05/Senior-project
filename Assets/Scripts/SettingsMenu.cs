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
        //Gets volumes from database, puts them into the audiomixer
        connection = new SqliteConnection(string.Format("URI=file:Assets/Streaming Assets/{0}.db", dbName));

        connection.Open();

        IDataReader dataReader = ReadSavedData();

        while (dataReader.Read())
        {

            // playerTransform.position = new Vector3(dataReader.GetFloat(1), dataReader.GetFloat(2), dataReader.GetFloat(3));
            musicSlider.value = dataReader.GetFloat(1);
            soundSlider.value = dataReader.GetFloat(2);
            audioMixer.SetFloat("musicVolume", Mathf.Log10(dataReader.GetFloat(1)) * 20);
            audioMixer.SetFloat("soundsVolume", Mathf.Log10(dataReader.GetFloat(2)) * 20);


        }
        connection.Close();



        //Sets resolution dropdown.
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


  
    //Set current resolution 
    public void setResolution   (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //set music volume
   public void SetMusicVolume (float volume)
    {
        
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);

    



    }

    //Sets sound volume
   public void SetSoundVolume(float volume)
   {
        audioMixer.SetFloat("soundsVolume", Mathf.Log10(volume) * 20);



    }
    //Set graphics quality
    public void SetQuality (int qualityIndex)
    {
        
        QualitySettings.SetQualityLevel(qualityIndex);


    }

    //Toggle Fullscreen
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
    
    
    IDataReader ReadSavedData()
    {
        // Create command (query)
        IDbCommand command = connection.CreateCommand();
        // Get all data in Slot = 1 from coordinates table
        command.CommandText = "SELECT * FROM Settings WHERE current = 1;";
        // Execute command
        IDataReader dataReader = command.ExecuteReader();
        return dataReader;
    }


    //Saves music and sound to database
    public void MusicSave()
    {
        float music = musicSlider.value;
        float sound = soundSlider.value;
        
        connection.Open();
     
        PushCommand(string.Format("UPDATE Settings SET music = {0}, sound = {1}  WHERE Current = 1;", music, sound), connection);
        
        
        


        
        UnityEngine.Debug.Log(music);
        UnityEngine.Debug.Log(sound);
        connection.Close();


    }

}
