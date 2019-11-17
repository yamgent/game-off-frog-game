using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    private const string SCORE_TEXT_FORMAT = "Steps:{0:D4}\nHigh:{1:D4}";

    private static Score singleton;
    
    private int currentScore;
    private static int highScore = 0;

    public Text scoreText;

    // Start is called before the first frame update
    void Start() {
        if (singleton != null) {
            Debug.LogError("Multiple Score managers found but should only have one!");
        }
        singleton = this;

        currentScore = 0;
        UpdateScoreText();
    }

    public static Score GetSingleton() {
        return singleton;
    }

    public void AddToCurrentScore(int scoreToAdd) {
        currentScore += scoreToAdd;
        if (currentScore > highScore) {
            highScore = currentScore;
        }
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.text = string.Format(SCORE_TEXT_FORMAT, currentScore, highScore);
    }
}
