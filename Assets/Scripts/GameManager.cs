using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Status
    {
	MENU,
	GAME,
	SCORES,
	RULES,
	GAMEOVER,
	EXIT
    }
    private Status m_status;

    public Text m_debugText;

    // Start is called before the first frame update
    void Start()
    {
	m_status = Status.MENU;
    }

    // Update is called once per frame
    void Update()
    {
	if(m_debugText)
	    m_debugText.text = GetStatusString();
    }

    public void GotoMenu() { m_status = Status.MENU; }
    public void GotoGame() { m_status = Status.GAME; }
    public void GotoScores() { m_status = Status.SCORES; }
    public void GotoRules() { m_status = Status.RULES; }
    public void GotoGameOver() { m_status = Status.GAMEOVER; }
    public void GotoExit() { m_status = Status.EXIT; }

    public string GetStatusString()
    {
	switch(m_status)
	{
	    case Status.MENU: { return "menu"; }
	    case Status.GAME: { return "game"; }
	    case Status.SCORES: { return "scores"; }
	    case Status.RULES: { return "rules"; }
	    case Status.GAMEOVER: { return "gameover"; }
	    case Status.EXIT: { return "exit"; }
	}
	return "Error";
    }
}
