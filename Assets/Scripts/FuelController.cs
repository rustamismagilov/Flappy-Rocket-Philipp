using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelController : MonoBehaviour
{
    PlayerController player;

    [HideInInspector] public float currentFuel = 10f;

    [SerializeField] float burnRate = 0.05f;
    [SerializeField] float maxFuel = 10f;

    [SerializeField] float addFuel = 20f;

    [SerializeField] Slider fuelSlider;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFuel;
            fuelSlider.value = currentFuel;
        }
    }

    void Update()
    {
        if (currentFuel <= 0)
        {
            currentFuel = 0;
        }

        if (fuelSlider != null)
        {
            fuelSlider.value = currentFuel;
        }
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

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            Debug.Log("Fuel collected");
            AddFuel(addFuel);
            Destroy(other.gameObject);
        }
    }
}
