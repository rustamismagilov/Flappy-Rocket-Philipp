using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // you need to set different index for each checkpoint game object!!!
    public Vector3 spawnOffset;
    private CheckpointSystem checkpointSystem;

    void Start()
    {
        checkpointSystem = FindObjectOfType<CheckpointSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint " + checkpointIndex);
            checkpointSystem.UpdateCheckpointIndex(checkpointIndex);
        }
    }

}
