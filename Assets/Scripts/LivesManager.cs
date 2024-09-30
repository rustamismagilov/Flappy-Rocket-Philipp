using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [Header("Lives Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] public int currentLives;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Audio")]
    [SerializeField] AudioClip crash;

    private float delay = 3f;

    void Awake()
    {
        // Implement Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentLives = maxLives; // Initialize currentLives
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateLivesText();
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign the UI elements after the scene loads
        // By searching for "LivesText" game object and doing everything through this game object
        livesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI>();
        UpdateLivesText();
    }

    public void LosingLives()
    {
        currentLives--;
        UpdateLivesText();

        if (currentLives < 0)
        {
            Invoke(nameof(GameOver), delay);
            currentLives = maxLives;
        }
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    public void GameOver()
    {
        //Debug.Log("Game Over");
        //GetComponent<PlayerController>().enabled = false;
        currentLives = maxLives; // Reset lives for a new game
        SceneManager.LoadScene(0); // Load the first scene or a Game Over scene
    }
}
