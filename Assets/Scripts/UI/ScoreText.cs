using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    private int m_Score;
    private Text m_ScoreText;
    public bool m_OnAnimate = false;

    void Start() {
        m_ScoreText = GetComponentInChildren<Text>();
        EventManager.Instance.Register("OnCurrentHeightSet", OnCurrentHeightSet);
    }

    public void SetScore(int newScore) {
        m_Score = newScore;
        m_ScoreText.text = "Score: " + m_Score.ToString();
        StartAnimate();
    }

    private void OnCurrentHeightSet(object currentHeight) {
        if (!(currentHeight is float current)) {
            return;
        }

        SetScore(m_Score + (int) current);
    }

    private void StartAnimate() {
        m_ScoreText.fontSize = 21;
        m_OnAnimate = true;
    }

    public int GetScore() {
        return m_Score;
    }

    private void ScoreAnimate() {
        if (m_ScoreText.fontSize <= 20) {
            m_ScoreText.fontSize = 20;
            m_OnAnimate = false;
        }
        else if (m_ScoreText.fontSize <= 30) {
            m_ScoreText.fontSize += 1;
        }
        else {
            m_ScoreText.fontSize = 20;
        }
    }

    private void FixedUpdate() {
        if (m_OnAnimate) {
            ScoreAnimate();
        }
    }
}