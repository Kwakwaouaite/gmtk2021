using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    private static EndGameManager s_instance = null;
    public static EndGameManager Instance
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

    [SerializeField]
    private float m_MaxLoveGauge = 100;
    [SerializeField]
    private float m_DecreasingLoveGaugeStartingSpeed = .5f;
    [SerializeField]
    private float m_DecreasingLoveGaugeEvolutingSpeed = .1f;
    [SerializeField]
    private float m_GainLoveGaugePerPoint = 1;
    [SerializeField]
    private bool m_CanSurpassMaxLoveGauge = false;

    [SerializeField]
    private float m_MaxTime = 100;

    [SerializeField]
    private int m_nbrBullets = 50;

    public Text timerText;
    public Text loveAmountText;
    public Text nbBulletsText;

    private float m_CurrentLoveGauge;
    private float m_CurrentLoveGaugeDecreasingSpeed;
    private float m_CurrentTime;
    private int m_CurrentNbrBullets;

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        m_CurrentNbrBullets = m_nbrBullets;
        m_CurrentTime = m_MaxTime;
        m_CurrentLoveGauge = m_MaxLoveGauge;
        m_CurrentLoveGaugeDecreasingSpeed = m_DecreasingLoveGaugeStartingSpeed;
        UpdateNbrBullets();
        ScoreManager.Instance.ResetScore();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_MaxLoveGauge > 0)
        {
            m_CurrentLoveGauge -= Time.fixedDeltaTime * m_CurrentLoveGaugeDecreasingSpeed;
            m_CurrentLoveGaugeDecreasingSpeed += Time.fixedDeltaTime * m_DecreasingLoveGaugeEvolutingSpeed;
            UpdateLoveGauge();
            if (m_CurrentLoveGauge <= 0)
            {
                EndGame();
            }
        }

        if (m_MaxTime > 0)
        {
            m_CurrentTime -= Time.fixedDeltaTime;
            UpdateTimer();
            if (m_CurrentTime <= 0)
            {
                EndGame();
            }
        }
        if (m_nbrBullets > 0 && m_CurrentNbrBullets <= 0)
        {
            EndGame();
        }
    }

    public void GainLoveGauge(int points)
    {
        if(m_MaxLoveGauge > 0)
        {
            m_CurrentLoveGauge += m_GainLoveGaugePerPoint * points;
            if(!m_CanSurpassMaxLoveGauge && m_CurrentLoveGauge > m_MaxLoveGauge)
            {
                m_CurrentLoveGauge = m_MaxLoveGauge;
            }
            UpdateLoveGauge();
        }
    }

    public void LoseBullet()
    {
        m_CurrentNbrBullets--;
        UpdateNbrBullets();
    }

    void UpdateTimer()
    {
        timerText.text = ((int)m_CurrentTime).ToString();
    }

    void UpdateLoveGauge()
    {
        loveAmountText.text = ((int)m_CurrentLoveGauge).ToString();
    }

    void UpdateNbrBullets()
    {
        nbBulletsText.text = m_CurrentNbrBullets.ToString();
    }

    void EndGame()
    {
        Time.timeScale = 0;
    }
}
