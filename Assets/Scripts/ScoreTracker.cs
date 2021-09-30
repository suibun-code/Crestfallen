using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : Singleton<ScoreTracker>
{
    //Text
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreMultiplierText;

    //Player variables
    public int score = 0;
    public int combo = 0;
    public int scoreMultiplier = 1;

    public int scoreOnHit;

    public void Hit()
    {
        if (scoreMultiplier < 10)
            scoreMultiplier += 1;

        combo += 1;
        score += scoreOnHit * ScoreTracker.instance.scoreMultiplier;
    }

    public void ResetCombo()
    {
        //combo = 0;
        //scoreMultiplier = 1;
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        scoreText.SetText("Score: " + ScoreTracker.instance.score);
        comboText.SetText("Combo: " + ScoreTracker.instance.combo);
        scoreMultiplierText.SetText("Score Multiplier: " + ScoreTracker.instance.scoreMultiplier.ToString("F2") + "x");
    }
}
