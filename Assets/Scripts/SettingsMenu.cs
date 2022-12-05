using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
   public void SetMusicVolume (float volume)
    {
        audioMixer.SetFloat("musicVolume",volume);
            
            
                
          
   }


   public void SetSoundVolume(float volume)
   {
        audioMixer.SetFloat("soundsVolume", volume);




   }

    public void SetQuality (int qualityIndex)
    {
        
        QualitySettings.SetQualityLevel(qualityIndex);


    }



}
