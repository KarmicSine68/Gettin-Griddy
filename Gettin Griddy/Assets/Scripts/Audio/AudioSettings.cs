/******************************************************************************
 * Author: Brad Dixon
 * File Name: AudioSettings.cs
 * Creation Date: 3/11/2025
 * Brief: Controls sliders for volume
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private GameObject audioStuff;

    private void Start()
    {
        audioStuff.SetActive(true);
        LoadSettings();
        audioStuff.SetActive(false);
    }

    public void LoadSettings()
    {
        Slider[] sliders = FindObjectsOfType<Slider>();
        Debug.Log(sliders.Length);

        foreach (Slider i in sliders)
        {
            if (i.name.Contains("Master"))
            {
                if (PlayerPrefs.HasKey("master"))
                {
                    LoadVolume(i, "master");
                }
                else
                {
                    SetSlider(i);
                }
            }
            else if (i.name.Contains("Music"))
            {
                if (PlayerPrefs.HasKey("music"))
                {
                    LoadVolume(i, "music");
                }
                else
                {
                    SetSlider(i);
                }
            }
            else if (i.name.Contains("SFX"))
            {
                if (PlayerPrefs.HasKey("sfx"))
                {
                    LoadVolume(i, "sfx");
                }
                else
                {
                    SetSlider(i);
                }
            }
        }
    }

    public void SetSlider(Slider slider)
    {
        float volume = slider.value;

        string s = "";
        
        if(slider.name.Contains("Master"))
        {
            s = "master";
        }
        else if(slider.name.Contains("Music"))
        {
            s = "music";
        }
        else if(slider.name.Contains("SFX"))
        {
            s = "sfx";
        }

        mixer.SetFloat(s, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(s, volume);
    }

    private void LoadVolume(Slider slider, string s)
    {
        slider.value = PlayerPrefs.GetFloat(s);

        mixer.SetFloat(s, Mathf.Log10(slider.value) * 20);
    }
}
