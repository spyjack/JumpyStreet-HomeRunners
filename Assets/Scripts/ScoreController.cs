using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ScoreController : MonoBehaviour
{
    int highScoreNumber = 0;
    string HighScore;
    int score = 0;
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
        // Check High Score
        if (score > highScoreNumber)
        {
            
            StreamWriter Writer = new StreamWriter(scoreText, true);
            Writer.WriteLine(score);

        }
        
    }
}
