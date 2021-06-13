using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance = null;
    public static GameManager Instance
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

    public enum Status
    {
	MENU,
	GAME,
	PAUSE,
	SCORES,
	RULES,
	GAMEOVER,
	EXIT
    }
    private Status m_status;

    public PlayerInput m_playerInput;
    public Canvas m_pauseCanvas;
    public Canvas m_gameCanvas;
    public Canvas m_menuCanvas;
    public Canvas m_endMenuCanvas;

    public Text m_endScoreText;
    public Text m_debugText;

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
	    GotoMenu();
    }

    // Update is called once per frame
    void Update()
    {
	    if(m_debugText)
	        m_debugText.text = GetStatusString();
    }

    public void GotoMenu()
    {
        Time.timeScale = 1;
        m_status = Status.MENU;
        DeactivateGame();
        DisableAllCanvas();
        m_menuCanvas.enabled = true;
        player.ResetPosition();
    }
    public void GotoGame()
    {
        ActivateGame();
        EndGameManager.Instance.InitGame();
        player.ResetPosition();
    }
    public void GotoScores()	{ m_status = Status.SCORES; }
    public void GotoRules()	{ m_status = Status.RULES; }
    public void GotoGameOver()
    {
        int score = ScoreManager.Instance.GetScore();
        m_status = Status.GAMEOVER;
        DeactivateGame();
        DisableAllCanvas();
        m_endMenuCanvas.enabled = true;
        m_endScoreText.text = score.ToString();
    }
    public void GotoExit()
    { 
        m_status = Status.EXIT;
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void Pause()
    {
        Time.timeScale = 0;
        m_status = Status.PAUSE;
        DeactivateGame();
        DisableAllCanvas();
        m_pauseCanvas.enabled = true;
    }

    public void Unpause()
    {
        m_status = Status.GAME;
        ActivateGame();
    }

    private void ActivateGame()
    {
        Time.timeScale = 1;
        m_status = Status.GAME;
        DisableAllCanvas();
        m_gameCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        m_playerInput.actions.FindActionMap("Player").Enable();
    }

    private void DeactivateGame()
    {
        Cursor.lockState = CursorLockMode.None;
        m_playerInput.actions.FindActionMap("Player").Disable();
    }

    private void DisableAllCanvas()
    {
        m_gameCanvas.enabled = false;
        m_pauseCanvas.enabled = false;
        m_endMenuCanvas.enabled = false;
        m_menuCanvas.enabled = false;
    }

    public string GetStatusString()
    {
	switch(m_status)
	{
	    case Status.MENU: { return "menu"; }
	    case Status.GAME: { return "game"; }
	    case Status.PAUSE: { return "pause"; }
	    case Status.SCORES: { return "scores"; }
	    case Status.RULES: { return "rules"; }
	    case Status.GAMEOVER: { return "gameover"; }
	    case Status.EXIT: { return "exit"; }
	}
	return "Error";
    }
}
