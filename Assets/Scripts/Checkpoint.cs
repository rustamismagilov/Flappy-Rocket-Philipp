using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;
    private CheckpointSystem checkpointSystem;

    void Start()
    {
        checkpointSystem = FindObjectOfType<CheckpointSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointSystem.UpdateCheckpointIndex(checkpointIndex);
        }
    }
}
