using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class UIController : MonoBehaviour
{
    public GameObject helpMenu;
    public GameObject creditsMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        helpMenu.SetActive(false);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TerrainGen");
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
}
