using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ScoreController : MonoBehaviour
{
    int highScoreNumber = 0;
    string HighScore;
    [SerializeField]
    int score = 0;
    [SerializeField]
    Text scoreDisplay = null;
    [SerializeField]
    Text scoreDeathDisplay = null;
    string scoreText = "Assets/HighScoreHolder/ScoreKeeper";
    // Start is called before the first frame update
    void Start()
    {
        StreamReader Reader = new StreamReader(scoreText);
        highScoreNumber = Convert.ToInt32(Reader.ReadToEnd());
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
        // Check High Score
        if (score > highScoreNumber)
        {
            
            StreamWriter Writer = new StreamWriter(scoreText, true);
            Writer.WriteLine(score);

        }
        
    }
}
