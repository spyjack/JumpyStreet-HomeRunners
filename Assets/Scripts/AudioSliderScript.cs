using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [Header("Main Menu Music")]
    public Slider musicVolume;
    public AudioSource musicSound;

    [Header("Button Sounds")]
    public Slider buttonSoundVolume;
    public AudioSource buttonSound;

    [Header("SFX Sounds")]
    public Slider sfxSoundVolume;
    public AudioSource sfxSound;

    // Start is called before the first frame update
    void Awake()
    {
        musicVolume.value = PlayerPrefs.GetFloat("musicVolume", .25f);
        buttonSoundVolume.value = PlayerPrefs.GetFloat("buttonVolume", 1f);
        sfxSoundVolume.value = PlayerPrefs.GetFloat("sfxVolume", .5f);
    }

    public void OnSliderChange(float newValue)
    {
        PlayerPrefs.SetFloat("musicVolume", newValue);
        PlayerPrefs.SetFloat("buttonVolume", newValue);
        PlayerPrefs.SetFloat("sfxVolume", newValue);
    }

    // Update is called once per frame
    void Update()
    { 

        //////////***** MAIN MENU *****//////////
        musicSound.volume = musicVolume.value;

        //////////***** ALL BUTTONS *****//////////
        buttonSound.volume = buttonSoundVolume.value;

        //////////***** ALL SFX *****//////////
        sfxSound.volume = sfxSoundVolume.value;
    }

    public void OnSaveButtonClick()
    {
        PlayerPrefs.SetFloat("menuVolume", musicVolume.value);
        Debug.Log("Saved menu music volume..");

        PlayerPrefs.SetFloat("buttonVolume", buttonSoundVolume.value);
        Debug.Log("Saved button volume..");

        PlayerPrefs.SetFloat("sfxVolume", sfxSoundVolume.value);
        Debug.Log("Saved button volume..");
    }
}
