using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public Text screenScore;
    public Text pauseScore;
    public Text deathScore;
    public Text highScore;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        screenScore.text = "Score: " + score.ToString();
        highScore.text = PlayerPrefs.GetInt("highScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        // Increasing Score 
        score++;
        deathScore.text = score.ToString();
        pauseScore.text = score.ToString();
        screenScore.text = "Score: " + score.ToString();

        if(score > PlayerPrefs.GetInt("highScore", 0))
        {
            PlayerPrefs.SetInt("highScore", score);
            highScore.text = score.ToString();
        }
    }
}
