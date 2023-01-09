using UnityEngine.Audio;
using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        //Checks if there is already an audio manager in the scene
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }




        DontDestroyOnLoad(gameObject);

        //Adds sounds to correct information
        foreach (Sound s in sounds )
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;
           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
           s.source.loop = s.loop;
           s.source.outputAudioMixerGroup = s.group;

        }



    }

    //Plays sounds if not null
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        
        s.source.Play();

    }
    //Stops the sounds from playing
    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        
        s.source.Stop();
    }
    //PLays title theme on start.
    void Start ()
    {
        Play("titletheme");
    }
   


}
