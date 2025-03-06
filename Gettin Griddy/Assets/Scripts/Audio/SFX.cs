/******************************************************************************
 * Author: Brad Dixon
 * File Name: SFX.cs
 * Creation Date: 3/6/2025
 * Brief: A list of variables for each sound effect that customizes each SFX
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX
{
    //The name of the sound effect to be played
    //Used to identify the corresponding sound effect in the list of SFX
    public string audioName;

    //List of potential clips to choose from when a sound gets played
    //Will only play one sound if the list has a count of 1
    public AudioClip[] clips;

    //Used by the SFX manager to store the audio clip that will be played
    //or stopped
    [HideInInspector]
    public AudioClip clip;

    //A range for how loud the volume can be
    [Range(0f, 1f)]
    public float maxVolume;

    //A range for the pitch of the clip
    [Range(0f, 3f)]
    public float pitch;

    //If true, the audio clip will loop when done playing
    public bool willLoop;

    //The audio source the clip will play from
    [HideInInspector]
    public AudioSource source;
}
