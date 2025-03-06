/******************************************************************************
 * Author: Brad Dixon
 * File Name: SoundManager.cs
 * Creation Date: 3/6/2025
 * Brief: Used to call functions to either play or stop a sound
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<SFX> audioClips = new List<SFX>();

    /// <summary>
    /// Makes sure there is only one instance of the singleton and creates
    /// all the audio clips to be used
    /// </summary>
    private void Awake()
    {
        //Makes sure there is one singleton instance
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //Creates different game objects for each sound so sounds can overlap
        foreach(SFX i in audioClips)
        {
            GameObject temp = Instantiate(new GameObject(), transform);

            //Adds the audio source to the clip that will be played
            i.source = temp.AddComponent<AudioSource>();
            i.source.volume = i.maxVolume;
            i.source.pitch = i.pitch;
            i.source.playOnAwake = false;
            i.source.loop = i.willLoop;
            i.source.clip = i.clips[0];
        }
    }

    /// <summary>
    /// Plays the corresponding SFX to the name that gets passed
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        //Finds the audio clip that has the same name
        SFX sfx = audioClips[audioClips.FindIndex(i => i.audioName == name)];
        sfx.source.clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
        sfx.source.PlayOneShot(sfx.source.clip);
    }

    /// <summary>
    /// Stops  a sound effect that is playing
    /// </summary>
    /// <param name="name"></param>
    public void StopSFX(string name)
    {
        //Finds the audio clip that has the same name
        SFX sfx = audioClips[audioClips.FindIndex(i => i.audioName == name)];
        sfx.source.Stop();
    }
}
