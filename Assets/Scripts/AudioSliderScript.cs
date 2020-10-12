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

    // Start is called before the first frame update
    void Awake()
    {
        mainMenuMusicVolume.value = PlayerPrefs.GetFloat("menuVolume", .25f);
        buttonSoundVolume.value = PlayerPrefs.GetFloat("buttonVolume", 1f);
    }

    public void OnSliderChange(float newValue)
    {
        PlayerPrefs.SetFloat("menuVolume", newValue);
        PlayerPrefs.SetFloat("buttonVolume", newValue);
    }

    // Update is called once per frame
    void Update()
    { 

        //////////***** MAIN MENU *****//////////
        mainMenuMusic.volume = mainMenuMusicVolume.value;


        //////////***** ALL BUTTONS *****//////////
        buttonSound.volume = buttonSoundVolume.value;
    }

    public void OnSaveButtonClick()
    {
        PlayerPrefs.SetFloat("menuVolume", mainMenuMusicVolume.value);
        Debug.Log("Saved menu music volume..");

        PlayerPrefs.SetFloat("buttonVolume", buttonSoundVolume.value);
        Debug.Log("Saved button volume..");
    }
}
