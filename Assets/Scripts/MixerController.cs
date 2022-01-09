using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sfxSliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSliderValue) * 20);
    }
}
