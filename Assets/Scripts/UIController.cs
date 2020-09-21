using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class UIController : MonoBehaviour
{
    public GameObject helpMenu;
    public GameObject creditsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnStartButtonClick()
        {
        print("SceneManager.LoadScene('Level1');");
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
    }

    public void OnQuitButtonClick()
    {
        print("Quitting");
        Application.Quit();
    }


}
