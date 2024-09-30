using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1f;

    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    private bool standsOnFinishPlatform = false;
    private bool levelCompleted = false;
    private bool crashDetected = false;
    float timeOnFinishPlatform = 0;
    [SerializeField] float requiredTimeOnFinishPlatform = 3f;

    LivesManager livesManager;
    AudioSource audioSource;
    Powerup powerup;

    void Start()
    {
        livesManager = LivesManager.instance;

        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }

        powerup = FindObjectOfType<Powerup>();
    }

    void Update()
    {
        if (standsOnFinishPlatform && !levelCompleted)
        {
            timeOnFinishPlatform += Time.deltaTime;

            if (timeOnFinishPlatform >= requiredTimeOnFinishPlatform)
            {
                StartSuccessSequence();
            }
        }
        else if (!standsOnFinishPlatform)
        {
            timeOnFinishPlatform = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (levelCompleted || crashDetected) return;

        switch (other.gameObject.tag)
        {
            case "Start":
                Debug.Log("Start platform");
                break;

            case "Finish":
                standsOnFinishPlatform = true;
                break;

            case "Fuel":
                Destroy(other.gameObject);
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            standsOnFinishPlatform = false;
            timeOnFinishPlatform = 0;
        }
    }

    public void ReloadLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    void LoadNextLevel()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (totalScenes <= 0) return;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

        SceneManager.LoadScene(nextSceneIndex);
    }

    void StartSuccessSequence()
    {
        if (levelCompleted) return;
        levelCompleted = true;

        audioSource.PlayOneShot(success);
        GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(LoadNextLevel), delay);
    }

    void StartCrashSequence()
    {
        if (crashDetected) return;
        crashDetected = true;

        PlayerController playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            playerController.enabled = false;
            playerController.StopAllParticles();
            playerController.StopAllAudio();
        }

        audioSource.PlayOneShot(crash);
        GetComponent<PlayerController>().enabled = false;

        livesManager.LosingLives();

        if (livesManager.currentLives > 0)
        {
            Invoke(nameof(ReloadLevel), delay);
        }
    }
}
