using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1f;

    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    //Sound when PowerUp is Collected
    [SerializeField] AudioClip powerUpSound;

    [Header("Invincibility")]
    //Duration for how long invincibility lasts
    //[SerializeField] float invincibilityDuration = 10;

    private bool standsOnFinishPlatform = false;
    private bool levelCompleted = false;
    private bool crashDetected = false;
    private bool isInvincible = false;

    float timeOnFinishPlatform = 0;
    [SerializeField] float requiredTimeOnFinishPlatform = 3f;

    LivesManager livesManager;
    Renderer playerRenderer;
    AudioSource audioSource;

    void Start()
    {
        livesManager = LivesManager.instance;

        // If AudioSource is not on the same GameObject, find it in the scene
        if (audioSource == null)
        {
            audioSource = FindObjectOfType<AudioSource>();
        }
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
                //Debug.Log("This is the Start Platform");
                break;

            case "Finish":
                //Debug.Log("Currently on Finish Platform");
                standsOnFinishPlatform = true;
                break;

            case "Fuel":
                //Debug.Log("Fuel Collected");
                Destroy(other.gameObject);
                break;
            case "Powerup":
                ActivateInvincibility();
                Destroy(other.gameObject);
                break;

            case "Asteroid":
                if (isInvincible)
                {
                    Debug.Log("Is Invincible");
                    DestroyAsteroid(other.gameObject);
                }
                else
                {
                    StartCrashSequence();
                }
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
            Debug.Log("Left Finish Platform too soon");
            standsOnFinishPlatform = false;
            timeOnFinishPlatform = 0;
        }
    }

    public void ReloadLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);
    }

    void Respawn()
    {
        GetComponent<PlayerController>().enabled = true;
        CheckpointSystem checkpointSystem = FindObjectOfType<CheckpointSystem>();
        checkpointSystem.RespawnPlayer();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            Debug.Log("Rigidbody set to kinematic.");
        }
    }


    void LoadNextLevel()
    {
        //Debug.Log("Load next level");
        //Counts all the Scenes in the Game
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        //As soon as you played every Scene...
        if (totalScenes <= 0)
        {
            //This Debug.Log will show up...
            //Debug.Log("No more Scenes To Load");
            //and stops at this point
            return;
        }
        //Describes the Scene you currently in
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //adds 1 tho the current Scene.
        //The % prevents an If statemant and brings you back to the First Scene
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

        SceneManager.LoadScene(nextSceneIndex);
    }

    void ActivateInvincibility()
    {
        if (!isInvincible)
        {
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;

        Debug.Log("Invincibility Activated");
        yield return new WaitForSeconds(10);
        isInvincible = false;
        Debug.Log("Invincibility has ended");
    }

    void StartSuccessSequence()
    {
        if (levelCompleted) return;
        levelCompleted = true;

        //Debug.Log("Starting Success Sequence");
        audioSource.PlayOneShot(success);
        GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(LoadNextLevel), delay);
    }

    void StartCrashSequence()
    {
        Debug.Log("Crash detected.");
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

    void DestroyAsteroid(GameObject asteroid)
    {
        Destroy(asteroid);
    }
}
