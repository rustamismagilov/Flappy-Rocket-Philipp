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
        if (rb.isKinematic && Input.anyKey)
        {
            rb.isKinematic = false;
        }

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

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(rotationThust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(rotationThisFrame * Time.deltaTime * Vector3.forward);
        rb.freezeRotation = false;
    }

    public void StopAllParticles()
    {
        // Stops the main engine particle system
        if (mainEnginePS.isPlaying)
        {
            mainEnginePS.Stop();
        }
    }

    public void StopAllAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
