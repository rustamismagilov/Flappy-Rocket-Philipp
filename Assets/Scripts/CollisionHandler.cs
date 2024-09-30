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
    float timeOnFinishPlatform = 0;
    [SerializeField] float requiredTimeOnFinishPlatform = 3f;

    LivesManager livesManager;

    AudioSource audioSource;

    void Start()
    {
        livesManager = GetComponent<LivesManager>();
        audioSource = FindObjectOfType<AudioSource>();
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
        switch (other.gameObject.tag)
        {
            case "Start":
                Debug.Log("This is the Start Platform");
                break;

            case "Finish":
                Debug.Log("Currently on Finish Platform");
                standsOnFinishPlatform = true;
                break;

            case "Fuel":
                Debug.Log("Fuel Collected");
                Destroy(other.gameObject);
                break;

            default:
                Debug.Log("You hit Ground/Obstacle");
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

    void LoadNextLevel()
    {
        Debug.Log("Load next level");
        //Counts all the Scenes in the Game
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        //As soon as you played every Scene...
        if (totalScenes <= 0)
        {
            //This Debug.Log will show up...
            Debug.Log("No more Scenes To Load");
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

    void StartSuccessSequence()
    {
        if (levelCompleted) return;
        levelCompleted = true;

        Debug.Log("Starting Success Sequence");
        audioSource.PlayOneShot(success);
        GetComponent<PlayerController>().enabled = false;
        Invoke("LoadNextLevel", delay);
    }

    void StartCrashSequence()
    {
        audioSource.PlayOneShot(crash);
        GetComponent<PlayerController>().enabled = false;
        livesManager.LosingLives();

        if (livesManager.currentLives <= 0)
        {
            DeathSequence();
        }
    }

    void DeathSequence()
    {
        audioSource.PlayOneShot(crash);
        GetComponent<PlayerController>().enabled = false;
        Invoke("ReloadLevel", delay);
    }
}
