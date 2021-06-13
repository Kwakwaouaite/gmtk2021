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

    public Text m_debugText;

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
	    m_status = Status.MENU;
        m_menuCanvas.enabled = true;
        m_pauseCanvas.enabled = false;
        DeactivateGame();
    }
    public void GotoGame()
    {
        ActivateGame();
        EndGameManager.Instance.InitGame();
    }
    public void GotoScores()	{ m_status = Status.SCORES; }
    public void GotoRules()	{ m_status = Status.RULES; }
    public void GotoGameOver()	{ m_status = Status.GAMEOVER; }
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
        m_pauseCanvas.enabled = true;
        DeactivateGame();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        m_status = Status.GAME;
        m_pauseCanvas.enabled = false;
        ActivateGame();
    }

    private void ActivateGame()
    {
        m_status = Status.GAME;
        m_menuCanvas.enabled = false;
        m_pauseCanvas.enabled = false;
        m_gameCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        m_playerInput.actions.FindActionMap("Player").Enable();
    }

    private void DeactivateGame()
    {
        m_gameCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        m_playerInput.actions.FindActionMap("Player").Disable();
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
