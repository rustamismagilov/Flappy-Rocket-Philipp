using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    PlayerController player;

    [SerializeField] float currentFuel = 10f;

    [SerializeField] float burnRate = 0.05f;
    [SerializeField] float maxFuel = 10f;

    [SerializeField] GameObject Player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void FuelSystem()
    {
        currentFuel -= burnRate * Time.deltaTime;
        if (currentFuel <= 0)
        {
            OutOfFuel();
        }
    }

    public void OutOfFuel()
    {
        Time.timeScale = 0;
    }
}
