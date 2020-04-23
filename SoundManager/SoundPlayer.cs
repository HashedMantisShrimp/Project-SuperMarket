using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource mainSource; 
    public Sources[] audioSources;
    public Sound[] sounds;
    private AudioSource source;

   /* public AudioClip popUp;
    public AudioClip pickUpTrolley;
    public AudioClip trolley;
    public AudioClip outdoorJogging;
    public AudioClip outdoorWalking;*/

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCam = this.gameObject;

        mainCam.AddComponent<AudioSource>();
        mainSource = mainCam.GetComponent<AudioSource>();
    }


    public void PlaySoundClip(string soundName)
    {   
        mainSource.time = 0;
        mainSource.clip = FindSound(soundName);
        mainSource.Play();
    }

    public void PlaySoundClip(string soundName, bool loop) {
        mainSource.loop = loop == true ? true : false; 
        mainSource.time = 0;
        mainSource.clip = FindSound(soundName);
        mainSource.Play();
    }

    public void PlaySoundClip(string soundName, bool loop, string sourceName) //ability to loop sounds and choose which soruce to play in
    {
        source = FindSource(sourceName);
        source.loop = loop == true ? true : false;
        source.time = 0;
        source.clip = FindSound(soundName);
        source.Play();
    }

    public void StopSoundClip()
    {
        mainSource.Stop();
    }

    public void StopSoundClip( string sourceName)
    {
        source = FindSource(sourceName);
        source.Stop();
    }

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

    private AudioClip FindSound(string name)
    {
        Sound soundItem = Array.Find(sounds, sound => sound.title == name);
        if (soundItem == null)
        {
            Debug.Log("Sound " + name + " Wasnt found.");
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
            Debug.Log("Source " + sourceName + " Wasnt found.");
            return null;
        }
        else
        {
            return sourceItem.audioSource;
        }
    }
}
