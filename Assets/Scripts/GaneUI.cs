using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaneUI : MonoBehaviour
{
    public GameObject gameWinUI;
    public GameObject gameLoseUI;

    bool isGameOver;
	
	void Start ()
    {
        Guard.OnGuardSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<Controller>().OnFinish += ShowGameWinUI;
	}
	
	
	void Update ()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
    }

    void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        isGameOver = true;
        Guard.OnGuardSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<Controller>().OnFinish -= ShowGameWinUI;
    }
}
