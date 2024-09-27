using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    PlayerController player;

    [SerializeField] public float currentFuel = 10f;

    [SerializeField] float burnRate = 0.05f;
    [SerializeField] float maxFuel = 10f;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void ConsumeFuel()
    {
        currentFuel -= burnRate * Time.deltaTime;
    }

    public void OutOfFuel()
    {
        player.enabled = false;
        Debug.Log("Out of fuel");
        //Time.timeScale = 0;
    }
}
