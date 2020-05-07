using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{//This Script is suppoed to be assigned to the main camera
    public AudioSource mainSource; 
    public Sources[] audioSources;
    public Sound[] sounds;
    private AudioSource source;

    
    void Start()
    {
        GameObject mainCam = gameObject;
        mainCam.AddComponent<AudioSource>();
        mainSource = mainCam.GetComponent<AudioSource>();
        PlaySoundClip("Environment", true, "Env-Src");
    }

    #region PlaySoundClip Functions

    public void PlaySoundClip(string soundName)
    {   
        mainSource.time = 0;
        mainSource.clip = FindSound(soundName);
        mainSource.Play();
    }

    public void PlaySoundClip(string soundName, bool loop)
    {
        mainSource.loop = loop == true ? true : false; 
        mainSource.time = 0;
        mainSource.clip = FindSound(soundName);
        mainSource.Play();
    }

    public void PlaySoundClip(string soundName, bool loop, string sourceName)
    {//ability to loop sounds and choose which soruce to play in
        source = FindSource(sourceName);
        source.loop = loop == true ? true : false;
        source.time = 0;
        source.clip = FindSound(soundName);
        source.Play();
    }
    #endregion

    #region StopSoundClip Functions

    public void StopSoundClip()
    {
        mainSource.Stop();
    }

    public void StopSoundClip( string sourceName)
    {
        source = FindSource(sourceName);
        source.Stop();
    }
    #endregion

    public bool isSourcePlaying(string sourceName)
    {
        source = FindSource(sourceName);

        if (source.isPlaying)
        {
            return true;
        }
        else {
            return false;
        }
    }

    #region Find Functions

    private AudioClip FindSound(string name)
    {
        Sound soundItem = Array.Find(sounds, sound => sound.title == name);

        if (soundItem == null)
        {
            Debug.Log($"Sound {name} Wasnt found.");
            return null;
        }
        else
        {
            return soundItem.sound;
        }
    }

    private AudioSource FindSource(string sourceName) {
        Sources sourceItem = Array.Find(audioSources, source => source.name == sourceName);

        if (sourceItem == null)
        {
            Debug.Log($"Source {sourceName} Wasnt found.");
            return null;
        }
        else
        {
            return sourceItem.audioSource;
        }
    }
    #endregion
}
