using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    FuelController fuelController;

    [SerializeField] float mainThrust = 10f;
    [SerializeField] float rotationThust = 1f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEnginePS;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = FindAnyObjectByType<AudioSource>();
        fuelController = FindAnyObjectByType<FuelController>();
    }

    void Update()
    {
        ProcessRotation();
        ProcessThrust();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (fuelController.currentFuel > 0)
            {
                fuelController.ConsumeFuel();

                rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(mainEngine);
                }
                if (!mainEnginePS.isPlaying)
                {
                    mainEnginePS.Play();
                }
            }
            else
            {
                audioSource.Stop();
                mainEnginePS.Stop();
                fuelController.OutOfFuel();
            }
        }
        else
        {
            audioSource.Stop();
            mainEnginePS.Stop();

            if (fuelController.currentFuel <= 0)
            {
                fuelController.OutOfFuel();
            }
        }
    }



    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(-rotationThust);
            Debug.Log("Turn Left");
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(rotationThust);
            Debug.Log("Turn Right");
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
