using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager s_instance = null;
    public static ScoreManager Instance
    {
        get
        {
            return s_instance;
        }
    }
    private void Awake()
    {
        // if the singleton has already been initialized
        if (s_instance != null && s_instance != this)
        {
            Destroy(this.gameObject);
        }
        s_instance = this;
    }

    private Text scoreText;
    private int m_Score = 0;

    private void Start()
    {
        scoreText = GetComponent<Text>();
    }

    public void GainPoints(int nbPeople, int nbcommonProps)
    {
        GainPoints(nbPeople * nbcommonProps * nbcommonProps);
    }

    public void GainPoints(int points)
    {
        m_Score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = m_Score.ToString();
    }
}
