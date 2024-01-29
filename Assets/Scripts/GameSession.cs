using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] float levelLoadLevel = 2f;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] TextMeshProUGUI scoreText;
    public int playerLives = 3;
    public int score = 0;
    void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if(numGameSession > 1){
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Start() 
    {
        liveText.text = 'x' + playerLives.ToString();
        scoreText.text = score.ToString();
    }
    public void Score(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }
    public void ProccesPlayerDeath()
    {
        if(playerLives > 1)
        {
            StartCoroutine(TakeLife());
        }
        else
        {
            StartCoroutine(ResetGameSession());
        }
    }
    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(levelLoadLevel);
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        liveText.text = 'x' + playerLives.ToString();
    }
        IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(levelLoadLevel);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
