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
    public float score = 0;
    public int combo = 0;
    public float scoreMultiplier = 1.0f;

    public void ResetCombo()
    {
        ScoreTracker.instance.combo = 0;
        ScoreTracker.instance.scoreMultiplier = 1;
        ScoreTracker.instance.UpdateTexts();
    }

    public void UpdateTexts()
    {
        ScoreTracker.instance.scoreText.SetText("Score: " + ScoreTracker.instance.score);
        ScoreTracker.instance.comboText.SetText("Combo: " + ScoreTracker.instance.combo);
        ScoreTracker.instance.scoreMultiplierText.SetText("Score Multiplier: " + ScoreTracker.instance.scoreMultiplier.ToString("F2") + "x");
    }
}
