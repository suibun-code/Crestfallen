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
    public TextMeshProUGUI accuracyText;

    //Player variables
    public int score = 0;
    public int combo = 0;
    public int scoreMultiplier = 1;

    public int scoreOnPerfect = 100;
    public int scoreOnGreat = 50;
    public int scoreOnBad = 30;

    public void HitPerfect()
    {
        if (scoreMultiplier < 10)
            scoreMultiplier += 1;

        combo += 1;
        score += scoreOnPerfect * ScoreTracker.instance.scoreMultiplier;

        accuracyText.SetText("Perfect");
        accuracyText.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        UpdateTexts();
    }

    public void HitGreat()
    {
        if (scoreMultiplier < 10)
            scoreMultiplier += 1;

        combo += 1;
        score += scoreOnGreat * ScoreTracker.instance.scoreMultiplier;

        accuracyText.SetText("Great");
        accuracyText.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        UpdateTexts();
    }

    public void HitBad()
    {
        if (scoreMultiplier < 10)
            scoreMultiplier += 1;

        combo += 1;
        score += scoreOnBad * ScoreTracker.instance.scoreMultiplier;

        accuracyText.SetText("Bad");
        accuracyText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        UpdateTexts();
    }
    public void HitMiss()
    {
        ResetCombo();
        accuracyText.SetText("Miss");
        accuracyText.color = new Color(0.66f, 0.18f, 0.85f, 1.0f);
    }

    private void ResetCombo()
    {
        combo = 0;
        scoreMultiplier = 1;
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        scoreText.SetText("Score: " + ScoreTracker.instance.score);
        comboText.SetText("Combo: " + ScoreTracker.instance.combo);
        scoreMultiplierText.SetText("Score Multiplier: " + ScoreTracker.instance.scoreMultiplier.ToString("F2") + "x");
    }
}
