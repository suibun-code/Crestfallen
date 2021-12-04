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

    public int maxComboMultiplier = 3;
    public int scoreOnPerfect = 100;
    public int scoreOnGreat = 50;
    public int scoreOnBad = 30;

    //Player variables
    [ReadOnly] public int score = 0;
    [ReadOnly] public int combo = 0;
    [ReadOnly] public int scoreMultiplier = 1;

    public void Hit(string text, Color color, int scoreToGive)
    {
        if (scoreMultiplier < maxComboMultiplier)
            scoreMultiplier += 1;

        combo += 1;
        score += scoreToGive * ScoreTracker.instance.scoreMultiplier;

        accuracyText.SetText(text);
        accuracyText.color = color;
        UpdateTexts();
    }

    public void HitPerfect()
    {
        Hit("Perfect", AccuracyColor.perfect, scoreOnPerfect);
    }

    public void HitGreat()
    {
        Hit("Great", AccuracyColor.great, scoreOnGreat);
    }

    public void HitBad()
    {
        Hit("Bad", AccuracyColor.bad, scoreOnBad);
    }

    public void HitMiss()
    {
        ResetCombo();
        accuracyText.SetText("Miss");
        accuracyText.color = AccuracyColor.miss;
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
