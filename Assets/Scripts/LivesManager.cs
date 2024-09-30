using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    [Header("Lives Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] public int currentLives;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI livesText;

    CollisionHandler collisionHandler;
    void Start()
    {
        collisionHandler = GetComponent<CollisionHandler>();

        int numLivesManager = FindObjectsOfType<LivesManager>().Length;
        if (numLivesManager > 1)
        {
            Destroy(gameObject);
        }

        //currentLives = maxLives;
        LivesUI();

        livesText.text = "Lives: " + currentLives;

    }

    void Update()
    {

    }

    public void LosingLives()
    {
        currentLives--;
        LivesUI();

        if (currentLives > 0)
        {
            collisionHandler.ReloadLevel();
        }
        else
        {
            GameOver();
        }
    }

    void LivesUI()
    {
        livesText.text = "Lives: " + currentLives;
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene(0);
        //currentLives = maxLives;
    }
}
