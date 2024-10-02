using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public Checkpoint[] checkpoints;

    private int currentCheckpointIndex = -1;
    private GameObject player;
    private Rigidbody playerRb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    public void UpdateCheckpointIndex(int checkpointIndex)
    {
        if (checkpointIndex >= 0 && checkpointIndex < checkpoints.Length)
        {
            currentCheckpointIndex = checkpointIndex;
            Debug.Log("Checkpoint updated to index: " + currentCheckpointIndex);
        }
    }

    public void RespawnPlayer()
    {
        if (currentCheckpointIndex >= 0 && currentCheckpointIndex < checkpoints.Length)
        {
            Vector3 spawnPosition = checkpoints[currentCheckpointIndex].transform.position
                                    + checkpoints[currentCheckpointIndex].spawnOffset;
            player.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0, 90, 0));
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            Debug.Log("Player respawned at checkpoint " + currentCheckpointIndex);
        }
        else
        {
            Debug.LogWarning("Invalid checkpoint index! Cannot respawn player.");
        }
    }
}
