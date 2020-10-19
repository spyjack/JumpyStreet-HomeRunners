using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [Header("Main Menu Music")]
    public Slider mainMenuMusicVolume;
    public AudioSource mainMenuMusic;

    [Header("Button Sounds")]
    public Slider buttonSoundVolume;
    public AudioSource buttonSound;

    [Header("SFX Sounds")]
    public Slider sfxSoundVolume;
    public AudioSource sfxSound;

    // Start is called before the first frame update
    void Awake()
    {
        mainMenuMusicVolume.value = PlayerPrefs.GetFloat("menuVolume", .25f);
        buttonSoundVolume.value = PlayerPrefs.GetFloat("buttonVolume", 1f);
        sfxSoundVolume.value = PlayerPrefs.GetFloat("sfxVolume", .5f);
    }

    public void OnSliderChange(float newValue)
    {
        PlayerPrefs.SetFloat("menuVolume", newValue);
        PlayerPrefs.SetFloat("buttonVolume", newValue);
        PlayerPrefs.SetFloat("sfxVolume", newValue);
    }

    // Update is called once per frame
    void Update()
    { 

        //////////***** MAIN MENU *****//////////
        mainMenuMusic.volume = mainMenuMusicVolume.value;

        //////////***** ALL BUTTONS *****//////////
        buttonSound.volume = buttonSoundVolume.value;

        //////////***** ALL SFX *****//////////
        sfxSound.volume = sfxSoundVolume.value;
    }

    public void OnSaveButtonClick()
    {
        PlayerPrefs.SetFloat("menuVolume", mainMenuMusicVolume.value);
        Debug.Log("Saved menu music volume..");

        PlayerPrefs.SetFloat("buttonVolume", buttonSoundVolume.value);
        Debug.Log("Saved button volume..");

        PlayerPrefs.SetFloat("sfxVolume", sfxSoundVolume.value);
        Debug.Log("Saved button volume..");
    }
}
