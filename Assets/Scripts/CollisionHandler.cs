using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1f;

    [SerializeField] AudioClip sucess;
    [SerializeField] AudioClip Crash;

    AudioSource audioSource;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Start":
                Debug.Log("This is the Start Platform");
                break;

            case "Finish":
                Debug.Log("This is the Finish Platform");
                StartSucessSequence();
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

    void ReloadLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex== SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void StartSucessSequence()
    {
        audioSource.PlayOneShot(sucess);
        GetComponent<PlayerController>().enabled = false;
        Invoke("LoadNextLevel", delay);
    }

    void StartCrashSequence()
    {
        audioSource.PlayOneShot(Crash);
        GetComponent<PlayerController>().enabled = false;
        Invoke("ReloadLevel", delay);
    }
}
