using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    PlayerController player;

    [HideInInspector] public float currentFuel;

    [SerializeField] float burnRate = 0.05f;
    [SerializeField] float maxFuel = 10f;
    [SerializeField] float fuelToAdd = 20f; // Amount of fuel to add when colliding with a fuel pickup

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentFuel = maxFuel; // Initialize current fuel to the maximum at the start
    }

    public void ConsumeFuel()
    {
        currentFuel -= burnRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel); // Clamp to ensure it doesn't go below 0
        Debug.Log(currentFuel);
    }

    public void OutOfFuel()
    {
        player.enabled = false;
        Debug.Log("Out of fuel");
        // add all other things related to crash sequence here



        //Time.timeScale = 0;
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel); // Clamp to ensure it doesn't exceed maxFuel
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            AddFuel(fuelToAdd);
            Destroy(other.gameObject);
        }
    }
}
