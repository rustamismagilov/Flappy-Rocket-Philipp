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

    AudioSource audioSource;

    void Start()
    {
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
            Debug.Log("Left Finish Platform too soon");
            standsOnFinishPlatform = false;
            timeOnFinishPlatform = 0;
        }
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

        // Call Respawn after delay
        Invoke("Respawn", delay);
    }

    void Respawn()
    {
        GetComponent<PlayerController>().enabled = true;

        CheckpointSystem checkpointSystem = FindObjectOfType<CheckpointSystem>();
        checkpointSystem.RespawnPlayer();

        // Set Rigidbody to isKinematic = true AFTER respawning
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void LoadNextLevel()
    {
        Debug.Log("Load next level");
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (totalScenes <= 0)
        {
            Debug.Log("No more Scenes To Load");
            return;
        }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

        SceneManager.LoadScene(nextSceneIndex);
    }
}
