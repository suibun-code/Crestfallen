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

    [SerializeField] private float monoSpaceAmount;

    //Colors
    public Color colorPerfect;
    public Color colorGreat;
    public Color colorBad;
    public Color colorMiss;

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
        Hit("PERFECT", colorPerfect, scoreOnPerfect);
    }

    public void HitGreat()
    {
        Hit("GREAT", colorGreat, scoreOnGreat);
    }

    public void HitBad()
    {
        Hit("BAD", colorBad, scoreOnBad);
    }

    public void HitMiss()
    {
        ResetCombo();
        accuracyText.SetText("MISS");
        accuracyText.color = colorMiss;
    }

    private void ResetCombo()
    {
        combo = 0;
        scoreMultiplier = 1;
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        scoreText.SetText($"<mspace={monoSpaceAmount}em>" + ScoreTracker.instance.score.ToString());
        comboText.SetText($"<mspace={monoSpaceAmount}em>" + ScoreTracker.instance.combo.ToString());
        scoreMultiplierText.SetText($"<mspace={monoSpaceAmount}em>" + ScoreTracker.instance.scoreMultiplier.ToString() + "x");
    }
}
