using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class UIController : MonoBehaviour
{
    public GameObject helpMenu;
    public GameObject creditsMenu;
    public GameObject puaseMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnPauseButtonClick()
    {
        PauseGame();
    }

    public void OnHelpButtonClick()
    {
        helpMenu.SetActive(true);
    }

    public void OnCreditsButtonClick()
    {
        creditsMenu.SetActive(true);
    }

    public void OnCloseButtonClick()
    {
        helpMenu.SetActive(false);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void OnSettingsButtonClick()
    {
        settingsMenu.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        print("Quitting");
        Application.Quit();
    }

    void PauseGame()
    {
        puaseMenu.SetActive(true);
        Time.timeScale = 0f;    
    }

    public void onResumeButtonClick()
    {
        puaseMenu.SetActive(false);
        Time.timeScale = 1f;        
    }


}
