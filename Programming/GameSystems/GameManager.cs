using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;
    private GUIManager gui;
    public float gameTime = 60;
    public int p1Score = 0;
    public int p2Score = 0;
    public bool startGametime = false;

    void Awake()
    {
        Instance = this;
    }

	void Start ()
    {
        gui = this.GetComponent<GUIManager>();
    }
	
	void Update ()
    {
        ClampScore();
        UpdateGUI();
        if (startGametime)
        { gameTime -= Time.deltaTime; }
        EndGame();
    }

    private void ClampScore()
    {
        p1Score = (int)Mathf.Clamp(p1Score, 0, 100);
        p2Score = (int)Mathf.Clamp(p2Score, 0, 100);
    }

    private void UpdateGUI()
    {
        gui.gameTime.text = "" + gameTime.ToString("f0");
        gui.p1Score.text = "" + p1Score;
        gui.p2Score.text = "" + p2Score;
    }

    private void EndGame()
    {
       if(gameTime < 0)
        {
            gameTime = 0;
            gui.afterActionMenu.SetActive(true);
            StopPlayerMovement();
        }
    }

    public void StopPlayerMovement()
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        for (int x = 0; x < players.Length; x++)
        {
            players[x].enabled = false;
            players[x].gameObject.GetComponent<PlayerShootingManager>().enabled = false;
        }
    }

    public void StartPlayerMovement()
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        for (int x = 0; x < players.Length; x++)
        {
            players[x].enabled = true;
            players[x].gameObject.GetComponent<PlayerShootingManager>().enabled = true;
        }
    }
}
