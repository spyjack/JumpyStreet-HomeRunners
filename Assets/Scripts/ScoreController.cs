using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public static int highScore;
    [SerializeField]
    Text highScoreText = null;
    [SerializeField]
    public static int score = 0;
    [SerializeField]
    Text scoreDisplay = null;
    [SerializeField]
    Text scoreDeathDisplay = null;
    [SerializeField]
    Text scorePauseDisplay = null;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = highScore.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        // Increasing Score 
        score++;
        scoreDisplay.text = "Score: " + score;
        scoreDeathDisplay.text = score.ToString();
        scorePauseDisplay.text = score.ToString();
    }

    public void ChangeHighScore(int newValue)
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", newValue);
        }
    }
}
